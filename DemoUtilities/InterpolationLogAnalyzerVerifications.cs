using ATFramework2._0.Utilities.Logs;
using ATFramework2._0.Utilities.Logs.Interpolation;
using LogEntry = ATFramework2._0.LogEntry;
using LogLevel = ATFramework2._0.LogLevel;

namespace DemoUtilities;

[TestFixture]
public class MinimumNormPolynomialInterpolatorVerifications
{
    [Test]
    public void Example2018_1_UnderdeterminedDoesNotPreserveLinearPolynomial()
    {
        // Kashpur & Khlobystov (2018), Example 1: nodes (0,1), (1,0), n=1, f(x,y)=1+2x+3y.
        double F(double x, double y) => 1.0 + 2.0 * x + 3.0 * y;
        double[][] nodes = [[0.0, 1.0], [1.0, 0.0]];
        var model = new MinimumNormPolynomialInterpolator(1).Fit(nodes, nodes.Select(n => F(n[0], n[1])));

        double px = 0.3, py = 0.4;
        double expected = (7.0 + 2.0 * px + 5.0 * py) / 3.0;
        Assert.That(model.Predict([px, py]), Is.EqualTo(expected).Within(1e-12));
        Assert.That(Math.Abs(model.Predict([px, py]) - F(px, py)), Is.GreaterThan(1e-6));
    }

    [Test]
    public void Example2018_2_PreservesLinearPolynomial()
    {
        double F(double x, double y) => 1.0 + 2.0 * x + 3.0 * y;
        double[][] nodes = [[0.0, 1.0], [1.0, 0.0], [0.0, -1.0]];
        var model = new MinimumNormPolynomialInterpolator(1).Fit(nodes, nodes.Select(n => F(n[0], n[1])));

        foreach (var p in new[] { new[] { 0.3, 0.4 }, [-0.25, 0.8], [2.0, -1.5] })
        {
            Assert.That(model.Predict(p), Is.EqualTo(F(p[0], p[1])).Within(1e-11));
        }
    }

    [Test]
    public void Example2016_FundamentalPolynomials()
    {
        // Kashpur & Khlobystov (2016): n=2, m=3, nodes (0,0), (2,0), (0,2).
        double[][] nodes = [[0.0, 0.0], [2.0, 0.0], [0.0, 2.0]];
        var model = new MinimumNormPolynomialInterpolator(2).Fit(nodes, [1.0, 2.0, 3.0]);

        double x = 0.3, y = 0.4;
        double[] expected =
        [
            1.0 - 0.1 * (x + y + 2 * x * x + 2 * y * y),
            0.1 * (x + 2 * x * x),
            0.1 * (y + 2 * y * y)
        ];

        var weights = model.FundamentalWeights([x, y]);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.That(weights[i], Is.EqualTo(expected[i]).Within(1e-12));
        }
    }

    [TestCase(new double[] { 1, 0, 1, 1, 1, 2, 2, 0, 2, 2, 3, 0 }, 128.0, 7675.48, TestName = "Handwritten node set A")]
    [TestCase(new double[] { 0, 0, 1, 0, 0, 1, 1, 1, 2, 0, 0, 2 }, 32.0, 372.57, TestName = "Handwritten node set B")]
    public void HandwrittenNodeSets_AreFullRankAndReproduceQuadratic(double[] flatNodes, double expectedDet, double expectedCond)
    {
        double Q(double x, double y) => 1 + 2 * x - 3 * y + 0.5 * x * x + 1.25 * x * y - 0.75 * y * y;
        var nodes = flatNodes.Chunk(2).ToArray();
        var model = new MinimumNormPolynomialInterpolator(2).Fit(nodes, nodes.Select(n => Q(n[0], n[1])));

        var d = model.Diagnostics;
        Assert.That(d.Rank, Is.EqualTo(6));
        Assert.That(d.PolynomialSpaceDimension, Is.EqualTo(6));
        Assert.That(d.UsedPseudoinverse, Is.False);
        Assert.That(d.Determinant, Is.EqualTo(expectedDet).Within(1e-6));
        Assert.That(d.ConditionNumber, Is.EqualTo(expectedCond).Within(0.01));

        foreach (var p in new[] { new[] { 0.2, 0.7 }, [1.4, 0.5], [2.1, 1.3], [-0.2, 0.4], [0.35, 0.65] })
        {
            Assert.That(model.Predict(p), Is.EqualTo(Q(p[0], p[1])).Within(1e-9));
        }
    }

    [Test]
    public void ThreeVariables_CompleteQuadraticCase_ReproducesPolynomial()
    {
        // k=3, n=2, m=p=C(5,3)=10.
        double Q(double[] v) =>
            1 + 2 * v[0] - 3 * v[1] + 0.5 * v[2] + 0.75 * v[0] * v[0] - 1.25 * v[0] * v[1] + 0.4 * v[0] * v[2]
            + 1.1 * v[1] * v[1] - 0.6 * v[1] * v[2] + 0.9 * v[2] * v[2];

        double[][] nodes =
        [
            [0, 0, 0], [1, 0, 0], [0, 1, 0], [0, 0, 1], [2, 0, 0],
            [0, 2, 0], [0, 0, 2], [1, 1, 0], [1, 0, 1], [0, 1, 1]
        ];
        var model = new MinimumNormPolynomialInterpolator(2).Fit(nodes, nodes.Select(Q));

        Assert.That(MinimumNormPolynomialInterpolator.PolynomialSpaceDimension(3, 2), Is.EqualTo(10));
        Assert.That(model.Diagnostics.Rank, Is.EqualTo(10));
        Assert.That(model.Diagnostics.IsComplete, Is.True);

        foreach (var p in new[] { new[] { 0.2, 0.3, 0.4 }, [-0.5, 0.25, 0.75], [1.2, -0.4, 0.1], [0.35, 0.65, -0.2] })
        {
            Assert.That(model.Predict(p), Is.EqualTo(Q(p)).Within(1e-9));
        }
    }

    [Test]
    public void LinearlyDependentNodes_UsePseudoinverse()
    {
        // Повтор вузла робить Γ виродженою.
        double[][] nodes = [[0.0, 1.0], [1.0, 0.0], [1.0, 0.0]];
        var model = new MinimumNormPolynomialInterpolator(1).Fit(nodes, [4.0, 3.0, 3.0]);

        Assert.That(model.Diagnostics.Rank, Is.EqualTo(2));
        Assert.That(model.Diagnostics.UsedPseudoinverse, Is.True);
        Assert.That(model.Diagnostics.Determinant, Is.EqualTo(0.0));
        Assert.That(model.Diagnostics.MaxNodeResidual, Is.LessThan(1e-9));
    }

    [Test]
    public void InconsistentNodes_ThrowWhenExactInterpolationRequired()
    {
        double[][] nodes = [[1.0, 0.0], [1.0, 0.0]];
        var model = new MinimumNormPolynomialInterpolator(1);

        Assert.Throws<InvalidOperationException>(() => model.Fit(nodes, [0.0, 1.0]));
        Assert.DoesNotThrow(() => model.Fit(nodes, [0.0, 1.0], requireExactInterpolation: false));
        Assert.That(model.Predict([1.0, 0.0]), Is.EqualTo(0.5).Within(1e-12));
    }
}

[TestFixture]
public class LogVectorInterpolationExperimentVerifications
{
    // Чисельний експеримент: x = (ERROR, TimeoutException, час очікування, кількість повторів),
    // нормалізація x̃ = (x₁, x₂, x₃/60, x₄/5), n = 1, k = 4, m = p = 5.
    private static readonly double[][] RawNodes =
    [
        [0, 0, 5, 0], [1, 0, 10, 0], [0, 1, 15, 1], [1, 1, 45, 5], [0, 0, 30, 3]
    ];

    private static readonly double[] NodeValues = [0.05, 0.35, 0.55, 0.95, 0.20];

    private static readonly MinMaxNormalizer Normalizer = new([0, 0, 0, 0], [1, 1, 60, 5]);

    private static MinimumNormPolynomialInterpolator Build() =>
        new MinimumNormPolynomialInterpolator(1).Fit(RawNodes.Select(Normalizer.Normalize), NodeValues);

    [Test]
    public void GammaDiagnostics_MatchReport()
    {
        var d = Build().Diagnostics;

        Assert.That(d.PolynomialSpaceDimension, Is.EqualTo(5));
        Assert.That(d.Rank, Is.EqualTo(5));
        Assert.That(d.Determinant, Is.EqualTo(0.00694444).Within(1e-7));
        Assert.That(d.ConditionNumber, Is.EqualTo(6367.89).Within(0.01));
        Assert.That(d.MaxNodeResidual, Is.LessThan(1e-12));
    }

    [Test]
    public void MainExample_GivesWeightsAndScoreFromReport()
    {
        var model = Build();
        var x = Normalizer.Normalize([1, 1, 30, 3]);

        var weights = model.FundamentalWeights(x);
        double[] expectedWeights = [0.2, 0.2, 0.2, 0.8, -0.4];
        for (int i = 0; i < expectedWeights.Length; i++)
        {
            Assert.That(weights[i], Is.EqualTo(expectedWeights[i]).Within(1e-9));
        }

        Assert.That(model.Predict(x), Is.EqualTo(0.87).Within(1e-9));
    }

    [TestCase(0, 0, 10, 0, 0.110)]
    [TestCase(1, 0, 30, 1, 0.540)]
    [TestCase(0, 1, 30, 2, 0.680)]
    [TestCase(1, 1, 30, 3, 0.870)]
    [TestCase(1, 1, 40, 4, 0.940)]
    [TestCase(1, 0, 45, 3, 0.620)]
    [TestCase(1, 1, 60, 5, 1.130)]
    public void OtherVectors_MatchReportTable(double x1, double x2, double x3, double x4, double expected)
    {
        Assert.That(Build().Predict(Normalizer.Normalize([x1, x2, x3, x4])), Is.EqualTo(expected).Within(1e-9));
    }
}

[TestFixture]
public class LogMetricsVerifications
{
    [Test]
    public void Extract_ParsesSevenMetricsFromMessage()
    {
        var metrics = LogMetricsExtractor.Extract(
            "TimeoutException: element not found after 30 s, retry 3; NullReferenceException; Validation failed: Expected 1 but was 2",
            LogLevel.Error,
            recentContextErrors: 2);

        Assert.That(metrics.ToVector(), Is.EqualTo(new double[] { 1, 1, 30, 3, 1, 1, 2 }));
    }

    [Test]
    public void Extract_TimeoutExceptionAloneIsNotOtherException()
    {
        var metrics = LogMetricsExtractor.Extract("WebDriverTimeoutException after 1500 ms", LogLevel.Warning);

        Assert.That(metrics.ErrorFlag, Is.EqualTo(0));
        Assert.That(metrics.TimeoutFlag, Is.EqualTo(1));
        Assert.That(metrics.ExceptionFlag, Is.EqualTo(0));
        Assert.That(metrics.WaitTimeSeconds, Is.EqualTo(1.5).Within(1e-12));
    }

    [Test]
    public void Serialize_RoundTrips()
    {
        var metrics = LogMetrics.FromVector([1, 0, 12.5, 2, 0, 3, 4]);

        Assert.That(LogMetrics.TryParse(metrics.Serialize(), out var parsed), Is.True);
        Assert.That(parsed!.ToVector(), Is.EqualTo(metrics.ToVector()));
    }

    [Test]
    public void LogWorker_StoresAndReloadsMetrics()
    {
        var path = Path.Combine(Path.GetTempPath(), $"interp-log-{Guid.NewGuid():N}.txt");
        try
        {
            var writer = new LogWorker(path);
            writer.Log("Login failed", LogLevel.Error, "LoginPage", "Auth");
            Thread.Sleep(5);
            writer.Log("Timeout after 20 s, retry 2", LogLevel.Error, "LoginPage", "Auth");
            Thread.Sleep(5);
            writer.Log("Step took 7 s", LogLevel.Info, "Home", "Nav");
            var expected = writer.GetAllLogs().Select(l => l.Metrics.ToVector()).ToList();
            writer.SaveLogsToFile();

            var reader = new LogWorker(path);
            var logs = reader.GetAllLogs();

            Assert.That(logs, Has.Count.EqualTo(3));
            Assert.That(expected[1], Is.EqualTo(new double[] { 1, 1, 20, 2, 0, 0, 1 }));
            Assert.That(expected[2], Is.EqualTo(new double[] { 0, 0, 7, 0, 0, 0, 0 }));
            for (int i = 0; i < logs.Count; i++)
            {
                Assert.That(logs[i].Metrics!.ToVector(), Is.EqualTo(expected[i]));
            }

            Assert.That(logs[1].Message, Is.EqualTo("Timeout after 20 s, retry 2"));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Test]
    public void LogWorker_LoadsLegacyFormatAndExtractsMetrics()
    {
        var path = Path.Combine(Path.GetTempPath(), $"interp-legacy-{Guid.NewGuid():N}.txt");
        try
        {
            var timestamp = DateTime.Now.ToString();
            File.WriteAllLines(path,
            [
                $"{timestamp}|Error|NetworkWorker|DatabaseSync|Timeout while waiting for response",
                "##########################"
            ]);

            var logs = new LogWorker(path).GetAllLogs();

            Assert.That(logs, Has.Count.EqualTo(1));
            Assert.That(logs[0].Metrics!.ErrorFlag, Is.EqualTo(1));
            Assert.That(logs[0].Metrics!.TimeoutFlag, Is.EqualTo(1));
        }
        finally
        {
            File.Delete(path);
        }
    }
}

[TestFixture]
public class InterpolationLogAnalyzerVerifications
{
    [Test]
    public void DefaultNodes_FormCompleteFullRankScheme()
    {
        var analyzer = new InterpolationLogAnalyzer([]);
        var d = analyzer.Diagnostics;

        Assert.That(d.FeatureDimension, Is.EqualTo(LogMetrics.Count));
        Assert.That(d.PolynomialSpaceDimension, Is.EqualTo(8));
        Assert.That(d.NodeCount, Is.EqualTo(8));
        Assert.That(d.Rank, Is.EqualTo(8));
        Assert.That(d.UsedPseudoinverse, Is.False);
        Assert.That(d.MaxNodeResidual, Is.LessThan(1e-9));
    }

    [Test]
    public void DefaultNodes_AreReproduced()
    {
        var analyzer = new InterpolationLogAnalyzer([]);
        foreach (var node in InterpolationLogAnalyzer.DefaultNodes)
        {
            Assert.That(analyzer.Evaluate(node.Metrics).RawScore, Is.EqualTo(node.Criticality).Within(1e-9), node.Description);
        }
    }

    [Test]
    public void DefaultNodes_GiveTransparentLinearContributions()
    {
        var analyzer = new InterpolationLogAnalyzer([], new InterpolationLogAnalyzerOptions { ClampScore = false });
        var nodes = InterpolationLogAnalyzer.DefaultNodes;
        double f0 = nodes[0].Criticality;

        // P₁(x̃) = f₀ + Σ (f_j − f₀) x̃_j
        double[] raw = [1, 1, 30, 3, 0, 1, 2];
        var normalized = InterpolationLogAnalyzer.DefaultNormalizer.Normalize(raw);
        double expected = f0 + normalized.Select((v, j) => (nodes[j + 1].Criticality - f0) * v).Sum();

        Assert.That(analyzer.Evaluate(LogMetrics.FromVector(raw)).RawScore, Is.EqualTo(expected).Within(1e-12));
        Assert.That(analyzer.Diagnostics.ConditionNumber, Is.LessThan(100));
    }

    [Test]
    public void Analyze_RanksSevereLogsAboveBenignOnes()
    {
        var now = DateTime.Now;
        var logs = new List<LogEntry>
        {
            new() { Timestamp = now, Level = LogLevel.Info, Message = "Page loaded successfully in 1 s", Context = "Home" },
            new() { Timestamp = now, Level = LogLevel.Error, Message = "Unhandled exception: NullReference", Context = "Login" },
            new() { Timestamp = now, Level = LogLevel.Critical, Message = "TimeoutException after 50 s, retry 4, fatal crash", Context = "Login" }
        };

        var results = new InterpolationLogAnalyzer(logs).AnalyzeDetailed();

        Assert.That(results[0].Score, Is.LessThan(results[1].Score));
        Assert.That(results[1].Score, Is.LessThan(results[2].Score));
        Assert.That(results[0].Category, Is.EqualTo("Low Priority"));
        Assert.That(results[2].Category, Is.EqualTo("Critical"));
        Assert.That(results.All(r => r.Score is >= 0 and <= 1), Is.True);
        Assert.That(results[2].Metrics.RecentContextErrors, Is.EqualTo(1), "previous Login error is counted");
    }

    [Test]
    public void ManuallyCreatedLogs_GetMetricsFilledAutomatically()
    {
        var now = DateTime.Now;
        var first = new LogEntry { Timestamp = now, Level = LogLevel.Error, Message = "Login failed", Context = "Login" };
        var second = new LogEntry { Timestamp = now, Level = LogLevel.Error, Message = "TimeoutException after 30 s, retry 3", Context = "Login" };

        // Без історії метрики виділяються з самого логу.
        Assert.That(second.Metrics.ToVector(), Is.EqualTo(new double[] { 1, 1, 30, 3, 0, 0, 0 }));

        _ = new InterpolationLogAnalyzer([first, second]);

        // Аналізатор заповнює метрики в логах з урахуванням історії контексту.
        Assert.That(second.Metrics.ToVector(), Is.EqualTo(new double[] { 1, 1, 30, 3, 0, 0, 1 }));
    }

    [Test]
    public void ClampScore_CanBeDisabled()
    {
        var analyzer = new InterpolationLogAnalyzer([], new InterpolationLogAnalyzerOptions { ClampScore = false });
        var result = analyzer.Evaluate(LogMetrics.FromVector([1, 1, 60, 5, 1, 5, 10]));

        Assert.That(result.Score, Is.EqualTo(result.RawScore));
    }
}

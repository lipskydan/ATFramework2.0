using System.Globalization;
using ATFramework2._0.Utilities.Logs;
using ATFramework2._0.Utilities.Logs.Interpolation;

namespace ATFramework2._0.Demo;

/// <summary>
/// Демонстрація аналізу логів методом поліноміальної інтерполяції мінімальної норми.
/// </summary>
public static class InterpolationAnalyzerDemo
{
    public static void Run()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        ReproduceReportExperiment();
        AnalyzeLogWorkerLogs();
        CompareWithLipsi();
    }

    /// <summary>
    /// Чисельний експеримент зі звіту: x = (ERROR, TimeoutException, час очікування, повтори), n = 1, m = p = 5.
    /// </summary>
    private static void ReproduceReportExperiment()
    {
        Console.WriteLine("====== Interpolation: numerical experiment (k = 4, n = 1) ======");

        double[][] nodes = [[0, 0, 5, 0], [1, 0, 10, 0], [0, 1, 15, 1], [1, 1, 45, 5], [0, 0, 30, 3]];
        double[] values = [0.05, 0.35, 0.55, 0.95, 0.20];
        var normalizer = new MinMaxNormalizer([0, 0, 0, 0], [1, 1, 60, 5]);

        var model = new MinimumNormPolynomialInterpolator(1).Fit(nodes.Select(normalizer.Normalize), values);
        Console.WriteLine(model.Diagnostics);

        var x = normalizer.Normalize([1, 1, 30, 3]);
        Console.WriteLine($"l(x̃) = ({string.Join(", ", model.FundamentalWeights(x).Select(w => w.ToString("F4")))})");
        Console.WriteLine($"P₁(1,1,30,3) = {model.Predict(x):F3}");

        foreach (var v in new double[][] { [0, 0, 10, 0], [1, 0, 30, 1], [0, 1, 30, 2], [1, 1, 40, 4], [1, 0, 45, 3], [1, 1, 60, 5] })
        {
            Console.WriteLine($"  ({string.Join(",", v)}) -> {model.Predict(normalizer.Normalize(v)):F3}");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Логи з LogWorker (7 метрик зберігаються автоматично) аналізуються InterpolationLogAnalyzer.
    /// </summary>
    private static void AnalyzeLogWorkerLogs()
    {
        Console.WriteLine("====== Interpolation: 7-metric log analysis ======");

        var path = Path.Combine(Path.GetTempPath(), "InterpolationDemoLog.txt");
        File.Delete(path);

        var logWorker = new LogWorker(path);
        logWorker.Log("Page loaded successfully in 1.2 s", LogLevel.Info, "Home", "Navigation");
        logWorker.Log("Retrying after slow response, attempt 2", LogLevel.Warning, "Dashboard", "Widgets");
        logWorker.Log("Validation failed: Expected 'Welcome' but was 'Error'", LogLevel.Error, "Login", "Auth");
        logWorker.Log("WebDriverTimeoutException after 30 s, retry 3", LogLevel.Error, "Login", "Auth");
        logWorker.Log("Unhandled exception: NullReference", LogLevel.Critical, "Login", "Auth");
        logWorker.Log("Element clicked after 500 ms", LogLevel.Info, "Profile", "UI");
        logWorker.SaveLogsToFile();

        var logs = new LogWorker(path).GetAllLogs();
        var analyzer = new InterpolationLogAnalyzer(logs);
        Console.WriteLine(analyzer.Diagnostics);

        foreach (var result in analyzer.AnalyzeDetailed())
        {
            Console.WriteLine($"{result.Score:F3} | {result.Category,-13} | x = ({string.Join(", ", result.Metrics.ToVector())}) | {result.Log.Message}");
        }

        File.Delete(path);
        Console.WriteLine();
    }

    /// <summary>
    /// Порівняння з LIPSI на розміченому наборі логів.
    /// </summary>
    private static void CompareWithLipsi()
    {
        Console.WriteLine("====== Interpolation vs LIPSI (labeled set) ======");

        var labeledLogs = LipsiVsMInetDemo.CreateLabeledLogs();
        var logs = labeledLogs.Keys.ToList();
        var expected = labeledLogs.Values.ToList();

        PrintAccuracy("Interpolation", expected, new InterpolationLogAnalyzer(logs).Analyze());
        PrintAccuracy("LIPSI", expected, new LipsiLogAnalyzer(logs).Analyze());
    }

    private static void PrintAccuracy(string name, List<string> expected, List<string> actual)
    {
        double micro = expected.Zip(actual).Count(p => p.First == p.Second) / (double)expected.Count;
        double macro = expected
            .Select((label, i) => (label, correct: label == actual[i]))
            .GroupBy(p => p.label)
            .Average(g => g.Count(p => p.correct) / (double)g.Count());

        Console.WriteLine($"{name,-14} MicroAccuracy: {micro:P2} | MacroAccuracy: {macro:P2}");
    }
}

namespace ATFramework2._0.Utilities.Logs.Interpolation;

/// <summary>
/// Діагностика інтерполяційної схеми: степінь, кількість вузлів, розмірність ознак,
/// розмірність простору поліномів, ранг, визначник, число обумовленості Γ,
/// використання псевдообернення та максимальна нев'язка у вузлах.
/// </summary>
public sealed record InterpolationDiagnostics(
    int Degree,
    int NodeCount,
    int FeatureDimension,
    long PolynomialSpaceDimension,
    int Rank,
    double Determinant,
    double ConditionNumber,
    bool UsedPseudoinverse,
    double MaxNodeResidual)
{
    /// <summary>Повний випадок m = p (за належного вибору вузлів відтворює поліноми степеня ≤ n).</summary>
    public bool IsComplete => NodeCount == PolynomialSpaceDimension;

    public override string ToString() =>
        $"n={Degree}, k={FeatureDimension}, m={NodeCount}, p={PolynomialSpaceDimension}, rank(Γ)={Rank}, " +
        $"det(Γ)={Determinant:G6}, cond(Γ)={ConditionNumber:G6}, " +
        $"inverse={(UsedPseudoinverse ? "Moore-Penrose pseudoinverse" : "Γ⁻¹")}, max node residual={MaxNodeResidual:E3}";
}

/// <summary>
/// Інтерполянт мінімальної норми (О. Ф. Кашпур, В. В. Хлобистов) для функції k змінних.
/// Ядро:              K_n(x, z) = Σ_{r=0..n} ⟨x, z⟩^r
/// Матриця Грама:     Γ_ij = K_n(x_i, x_j)
/// Вектор ядра:       k(x)_i = K_n(x_i, x)
/// Фундаментальні ваги Лагранжа: l(x) = Γ⁺ k(x)
/// Інтерполянт:       P_n(x) = fᵀ Γ⁺ k(x) = Σ l_i(x) f_i
/// де Γ⁺ = Γ⁻¹ для невиродженої Γ, інакше — псевдообернена матриця Мура–Пенроуза.
/// </summary>
public sealed class MinimumNormPolynomialInterpolator
{
    private double[][]? _nodes;
    private double[]? _values;
    private double[,]? _gamma;
    private double[,]? _gammaOperator;
    private InterpolationDiagnostics? _diagnostics;

    /// <param name="degree">Степінь полінома n (≥ 0).</param>
    /// <param name="rcond">Відносний поріг для власних значень Γ при визначенні рангу та псевдообернення.</param>
    /// <param name="interpolationTolerance">Допустима нев'язка у вузлах при вимозі точної інтерполяції.</param>
    public MinimumNormPolynomialInterpolator(int degree, double rcond = 1e-12, double interpolationTolerance = 1e-9)
    {
        if (degree < 0) throw new ArgumentOutOfRangeException(nameof(degree), "degree must be >= 0");

        Degree = degree;
        RCond = rcond;
        InterpolationTolerance = interpolationTolerance;
    }

    public int Degree { get; }
    public double RCond { get; }
    public double InterpolationTolerance { get; }

    /// <summary>Розмірність простору поліномів від k змінних степеня ≤ n: p = C(n + k, k).</summary>
    public static long PolynomialSpaceDimension(int featureDimension, int degree)
    {
        // C(n + k, k) = Π_{i=1..k} (n + i) / i — кожен проміжний результат є цілим.
        long result = 1;
        for (int i = 1; i <= featureDimension; i++)
        {
            result = result * (degree + i) / i;
        }

        return result;
    }

    /// <summary>Ядро K_n(x, z) = 1 + ⟨x, z⟩ + … + ⟨x, z⟩ⁿ.</summary>
    public double Kernel(IReadOnlyList<double> x, IReadOnlyList<double> z)
    {
        if (x.Count != z.Count) throw new ArgumentException("x and z must have the same dimension");

        double dot = 0;
        for (int i = 0; i < x.Count; i++) dot += x[i] * z[i];

        double result = 1.0;
        double term = 1.0;
        for (int r = 0; r < Degree; r++)
        {
            term *= dot;
            result += term;
        }

        return result;
    }

    /// <summary>
    /// Побудова інтерполянта за вузлами x_i та значеннями f_i.
    /// </summary>
    /// <param name="requireExactInterpolation">
    /// Якщо true — кидає виняток, коли нев'язка у вузлах перевищує допуск
    /// (наприклад, вузли лінійно залежні у просторі поліномів).
    /// </param>
    public MinimumNormPolynomialInterpolator Fit(
        IEnumerable<IReadOnlyList<double>> nodes,
        IEnumerable<double> values,
        bool requireExactInterpolation = true)
    {
        var nodesArr = nodes.Select(n => n.ToArray()).ToArray();
        var valuesArr = values.ToArray();

        if (nodesArr.Length == 0 || nodesArr[0].Length == 0)
            throw new ArgumentException("nodes must be a non-empty 2D array-like object", nameof(nodes));
        int k = nodesArr[0].Length;
        if (nodesArr.Any(n => n.Length != k))
            throw new ArgumentException("all nodes must have the same dimension", nameof(nodes));
        if (nodesArr.Any(n => n.Any(v => !double.IsFinite(v))))
            throw new ArgumentException("nodes contain NaN/Inf", nameof(nodes));
        if (valuesArr.Length != nodesArr.Length)
            throw new ArgumentException("values must contain one scalar value for each node", nameof(values));
        if (valuesArr.Any(v => !double.IsFinite(v)))
            throw new ArgumentException("values contain NaN/Inf", nameof(values));

        int m = nodesArr.Length;
        var gamma = new double[m, m];
        for (int i = 0; i < m; i++)
        {
            for (int j = i; j < m; j++)
            {
                gamma[i, j] = gamma[j, i] = Kernel(nodesArr[i], nodesArr[j]);
            }
        }

        // Γ симетрична та невід'ємно визначена, тому Γ = V Λ Vᵀ дає одночасно
        // ранг, визначник, число обумовленості, Γ⁻¹ та псевдообернену Γ⁺.
        var (eigenvalues, eigenvectors) = SymmetricEigen.Decompose(gamma);
        double maxEigen = eigenvalues.Max(Math.Abs);
        double threshold = RCond * maxEigen;
        int rank = eigenvalues.Count(l => l > threshold);
        bool fullRank = rank == m;

        var gammaOperator = new double[m, m];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < m; j++)
            {
                double sum = 0;
                for (int s = 0; s < m; s++)
                {
                    if (eigenvalues[s] > threshold)
                        sum += eigenvectors[i, s] * eigenvectors[j, s] / eigenvalues[s];
                }

                gammaOperator[i, j] = sum;
            }
        }

        _nodes = nodesArr;
        _values = valuesArr;
        _gamma = gamma;
        _gammaOperator = gammaOperator;

        double maxResidual = 0;
        for (int i = 0; i < m; i++)
        {
            maxResidual = Math.Max(maxResidual, Math.Abs(Predict(nodesArr[i]) - valuesArr[i]));
        }

        double minEigen = eigenvalues.Min();
        double determinant = fullRank ? eigenvalues.Aggregate(1.0, (acc, l) => acc * l) : 0.0;
        double condition = minEigen > 0 ? maxEigen / minEigen : double.PositiveInfinity;

        _diagnostics = new InterpolationDiagnostics(
            Degree,
            m,
            k,
            PolynomialSpaceDimension(k, Degree),
            rank,
            determinant,
            condition,
            !fullRank,
            maxResidual);

        if (requireExactInterpolation && maxResidual > InterpolationTolerance)
        {
            throw new InvalidOperationException(
                "The selected nodes/values do not yield exact interpolation within tolerance; " +
                $"max node residual = {maxResidual:E3}. Check linear independence of interpolation nodes " +
                "or allow a pseudoinverse approximation explicitly.");
        }

        return this;
    }

    /// <summary>Фундаментальні ваги l_i(x), i = 1..m, де l(x) = Γ⁺ k(x).</summary>
    public double[] FundamentalWeights(IReadOnlyList<double> x)
    {
        EnsureFitted();
        var kernelVector = KernelVector(x);
        int m = kernelVector.Length;

        var weights = new double[m];
        for (int i = 0; i < m; i++)
        {
            double sum = 0;
            for (int j = 0; j < m; j++) sum += _gammaOperator![i, j] * kernelVector[j];
            weights[i] = sum;
        }

        return weights;
    }

    /// <summary>Значення інтерполянта P_n(x) = Σ l_i(x) f_i.</summary>
    public double Predict(IReadOnlyList<double> x)
    {
        var weights = FundamentalWeights(x);
        double result = 0;
        for (int i = 0; i < weights.Length; i++) result += weights[i] * _values![i];
        return result;
    }

    public double[] PredictMany(IEnumerable<IReadOnlyList<double>> points) => points.Select(Predict).ToArray();

    /// <summary>Копія матриці Грама Γ.</summary>
    public double[,] Gamma
    {
        get
        {
            EnsureFitted();
            return (double[,])_gamma!.Clone();
        }
    }

    public InterpolationDiagnostics Diagnostics
    {
        get
        {
            EnsureFitted();
            return _diagnostics!;
        }
    }

    public bool IsFitted => _nodes != null;

    private double[] KernelVector(IReadOnlyList<double> x)
    {
        int k = _nodes![0].Length;
        if (x.Count != k)
            throw new ArgumentException($"expected a vector of dimension {k}, got {x.Count}", nameof(x));

        return _nodes.Select(node => Kernel(node, x)).ToArray();
    }

    private void EnsureFitted()
    {
        if (_nodes == null) throw new InvalidOperationException("Fit() must be called first");
    }
}

/// <summary>
/// Нормалізація ознак логів до [0, 1]^d за фіксованими межами навчальної вибірки:
/// x̃_j = (x_j − min_j) / (max_j − min_j).
/// </summary>
public sealed class MinMaxNormalizer
{
    private readonly double[] _min;
    private readonly double[] _max;

    public MinMaxNormalizer(IReadOnlyList<double> featureMin, IReadOnlyList<double> featureMax, bool clipToBounds = false)
    {
        if (featureMin.Count != featureMax.Count)
            throw new ArgumentException("featureMin and featureMax must have the same shape");
        if (featureMin.Zip(featureMax, (lo, hi) => hi - lo).Any(span => span <= 0))
            throw new ArgumentException("every feature must have featureMax > featureMin");

        _min = featureMin.ToArray();
        _max = featureMax.ToArray();
        ClipToBounds = clipToBounds;
    }

    /// <summary>Обрізати нормалізовані значення до [0, 1] (обмежує екстраполяцію).</summary>
    public bool ClipToBounds { get; }

    public IReadOnlyList<double> FeatureMin => _min;
    public IReadOnlyList<double> FeatureMax => _max;

    public double[] Normalize(IReadOnlyList<double> x)
    {
        if (x.Count != _min.Length)
            throw new ArgumentException("x, featureMin and featureMax must have the same shape", nameof(x));

        var result = new double[x.Count];
        for (int i = 0; i < x.Count; i++)
        {
            double value = (x[i] - _min[i]) / (_max[i] - _min[i]);
            result[i] = ClipToBounds ? Math.Clamp(value, 0.0, 1.0) : value;
        }

        return result;
    }
}

/// <summary>
/// Спектральний розклад симетричної матриці методом обертань Якобі: A = V Λ Vᵀ.
/// Точний та стійкий для невеликих матриць Грама (m — кількість вузлів).
/// </summary>
internal static class SymmetricEigen
{
    public static (double[] Eigenvalues, double[,] Eigenvectors) Decompose(double[,] matrix, int maxSweeps = 100)
    {
        int n = matrix.GetLength(0);
        var a = (double[,])matrix.Clone();
        var v = new double[n, n];
        for (int i = 0; i < n; i++) v[i, i] = 1.0;

        for (int sweep = 0; sweep < maxSweeps; sweep++)
        {
            double offDiagonal = 0;
            double diagonal = 0;
            for (int i = 0; i < n; i++)
            {
                diagonal += a[i, i] * a[i, i];
                for (int j = i + 1; j < n; j++) offDiagonal += a[i, j] * a[i, j];
            }

            if (offDiagonal <= 1e-30 * Math.Max(diagonal, double.Epsilon)) break;

            for (int p = 0; p < n - 1; p++)
            {
                for (int q = p + 1; q < n; q++)
                {
                    if (a[p, q] == 0) continue;

                    double theta = (a[q, q] - a[p, p]) / (2 * a[p, q]);
                    double t = Math.Sign(theta == 0 ? 1 : theta) / (Math.Abs(theta) + Math.Sqrt(theta * theta + 1));
                    double c = 1 / Math.Sqrt(t * t + 1);
                    double s = t * c;

                    for (int k = 0; k < n; k++)
                    {
                        double akp = a[k, p];
                        double akq = a[k, q];
                        a[k, p] = c * akp - s * akq;
                        a[k, q] = s * akp + c * akq;
                    }

                    for (int k = 0; k < n; k++)
                    {
                        double apk = a[p, k];
                        double aqk = a[q, k];
                        a[p, k] = c * apk - s * aqk;
                        a[q, k] = s * apk + c * aqk;
                    }

                    for (int k = 0; k < n; k++)
                    {
                        double vkp = v[k, p];
                        double vkq = v[k, q];
                        v[k, p] = c * vkp - s * vkq;
                        v[k, q] = s * vkp + c * vkq;
                    }
                }
            }
        }

        var eigenvalues = new double[n];
        for (int i = 0; i < n; i++) eigenvalues[i] = a[i, i];
        return (eigenvalues, v);
    }
}

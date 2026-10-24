namespace ATFramework2._0.Utilities.Logs.Interpolation;

/// <summary>
/// Інтерполяційний вузол: еталонний стан логу (7 сирих метрик) та показник критичності f ∈ [0, 1].
/// </summary>
public sealed record InterpolationNode(LogMetrics Metrics, double Criticality, string Description = "");

/// <summary>
/// Результат аналізу одного логу.
/// </summary>
/// <param name="Log">Вихідний лог.</param>
/// <param name="Metrics">Сирі метрики x.</param>
/// <param name="NormalizedMetrics">Нормалізований вектор x̃.</param>
/// <param name="RawScore">Значення інтерполянта P_n(x̃) (може виходити за [0, 1] при екстраполяції).</param>
/// <param name="Score">Показник критичності, обмежений [0, 1] (якщо увімкнено <see cref="InterpolationLogAnalyzerOptions.ClampScore"/>).</param>
/// <param name="Category">Категорія пріоритету.</param>
/// <param name="Weights">Фундаментальні ваги l(x̃) для кожного вузла.</param>
public sealed record InterpolationLogResult(
    LogEntry Log,
    LogMetrics Metrics,
    double[] NormalizedMetrics,
    double RawScore,
    double Score,
    string Category,
    double[] Weights);

/// <summary>
/// Налаштування інтерполяційного аналізатора.
/// </summary>
public sealed class InterpolationLogAnalyzerOptions
{
    /// <summary>Степінь полінома n. Для n = 1 та k = 7 повний випадок потребує p = C(8, 7) = 8 вузлів.</summary>
    public int Degree { get; init; } = 1;

    /// <summary>Інтерполяційні вузли. За замовчуванням — <see cref="InterpolationLogAnalyzer.DefaultNodes"/>.</summary>
    public IReadOnlyList<InterpolationNode>? Nodes { get; init; }

    /// <summary>Нормалізатор метрик. За замовчуванням — <see cref="InterpolationLogAnalyzer.DefaultNormalizer"/>.</summary>
    public MinMaxNormalizer? Normalizer { get; init; }

    /// <summary>Вимагати точного виконання інтерполяційних умов у вузлах.</summary>
    public bool RequireExactInterpolation { get; init; } = true;

    /// <summary>
    /// Обмежувати результат інтервалом [0, 1]. Звичайний поліноміальний інтерполянт не гарантує,
    /// що P_n(x) лишається в [0, 1] поза вузлами (особливо при екстраполяції).
    /// </summary>
    public bool ClampScore { get; init; } = true;

    /// <summary>Пороги категорій (за спаданням): Critical, High Priority, Normal; нижче — Low Priority.</summary>
    public double CriticalThreshold { get; init; } = 0.75;
    public double HighPriorityThreshold { get; init; } = 0.5;
    public double NormalThreshold { get; init; } = 0.25;
}

/// <summary>
/// Аналіз логів автоматизованого тестування методом поліноміальної інтерполяції мінімальної норми
/// (О. Ф. Кашпур, В. В. Хлобистов). Кожен лог перетворюється на вектор з 7 метрик (<see cref="LogMetrics"/>),
/// нормалізується до [0, 1]⁷, після чого значення інтерполянта
/// P_n(x̃) = fᵀ Γ⁺ k(x̃) трактується як показник критичності події.
/// </summary>
public class InterpolationLogAnalyzer
{
    private readonly List<LogEntry> _logs;
    private readonly InterpolationLogAnalyzerOptions _options;
    private readonly MinMaxNormalizer _normalizer;
    private readonly MinimumNormPolynomialInterpolator _interpolator;

    /// <summary>
    /// Межі нормалізації 7 метрик: прапорці лишаються без змін, час очікування ділиться на 60 с,
    /// кількість повторів — на 5, провалені перевірки — на 5, помилки в контексті — на 10.
    /// </summary>
    public static MinMaxNormalizer DefaultNormalizer { get; } = new(
        featureMin: [0, 0, 0, 0, 0, 0, 0],
        featureMax: [1, 1, 60, 5, 1, 5, 10]);

    /// <summary>
    /// Опорні логові стани (сирі метрики) та експериментальні показники критичності f_i ∈ [0, 1]:
    /// базовий стан без жодних ознак проблеми та по одному стану для кожної метрики на її верхній межі нормалізації.
    /// 8 вузлів = p = C(1 + 7, 7) — повний випадок для n = 1, тому Γ невироджена (cond(Γ) ≈ 10) і використовується Γ⁻¹.
    /// Для такого набору P₁(x̃) = f₀ + Σ_j (f_j − f₀) x̃_j, тобто внесок кожної метрики прозорий і монотонний.
    /// Порядок метрик: Error, Timeout, WaitTimeSeconds, RetryCount, Exception, FailedAssertions, RecentContextErrors.
    /// </summary>
    public static IReadOnlyList<InterpolationNode> DefaultNodes { get; } =
    [
        new(LogMetrics.FromVector([0, 0, 0, 0, 0, 0, 0]), 0.05, "Successful step"),
        new(LogMetrics.FromVector([1, 0, 0, 0, 0, 0, 0]), 0.50, "ERROR"),
        new(LogMetrics.FromVector([0, 1, 0, 0, 0, 0, 0]), 0.30, "TimeoutException"),
        new(LogMetrics.FromVector([0, 0, 60, 0, 0, 0, 0]), 0.20, "Long wait (60 s)"),
        new(LogMetrics.FromVector([0, 0, 0, 5, 0, 0, 0]), 0.20, "Many retries (5)"),
        new(LogMetrics.FromVector([0, 0, 0, 0, 1, 0, 0]), 0.35, "Exception / crash"),
        new(LogMetrics.FromVector([0, 0, 0, 0, 0, 5, 0]), 0.40, "Failed assertions (5)"),
        new(LogMetrics.FromVector([0, 0, 0, 0, 0, 0, 10]), 0.25, "Unstable context (10 recent errors)")
    ];

    public InterpolationLogAnalyzer(List<LogEntry>? logs, InterpolationLogAnalyzerOptions? options = null)
    {
        _logs = logs ?? [];
        LogMetricsExtractor.Fill(_logs);
        _options = options ?? new InterpolationLogAnalyzerOptions();
        _normalizer = _options.Normalizer ?? DefaultNormalizer;

        var nodes = _options.Nodes ?? DefaultNodes;
        _interpolator = new MinimumNormPolynomialInterpolator(_options.Degree).Fit(
            nodes.Select(n => (IReadOnlyList<double>)_normalizer.Normalize(n.Metrics.ToVector())),
            nodes.Select(n => n.Criticality),
            _options.RequireExactInterpolation);
    }

    /// <summary>Діагностика побудованої схеми (ранг, det, cond Γ, нев'язка у вузлах).</summary>
    public InterpolationDiagnostics Diagnostics => _interpolator.Diagnostics;

    /// <summary>Внутрішній інтерполянт (доступ до Γ, ваг тощо).</summary>
    public MinimumNormPolynomialInterpolator Interpolator => _interpolator;

    /// <summary>
    /// Основний метод аналізу: категорія пріоритету для кожного логу
    /// (Critical / High Priority / Normal / Low Priority) — сумісно з <see cref="LipsiLogAnalyzer"/>.
    /// </summary>
    public List<string> Analyze() => AnalyzeDetailed().Select(r => r.Category).ToList();

    /// <summary>
    /// Детальний аналіз: метрики, нормалізований вектор, значення інтерполянта, ваги вузлів та категорія.
    /// </summary>
    public List<InterpolationLogResult> AnalyzeDetailed()
    {
        LogMetricsExtractor.Fill(_logs);
        return _logs.Select(log => Evaluate(log.Metrics, log)).ToList();
    }

    /// <summary>Обчислює показник критичності для довільного вектора метрик.</summary>
    public InterpolationLogResult Evaluate(LogMetrics metrics, LogEntry? log = null)
    {
        var normalized = _normalizer.Normalize(metrics.ToVector());
        var weights = _interpolator.FundamentalWeights(normalized);
        double rawScore = _interpolator.Predict(normalized);
        double score = _options.ClampScore ? Math.Clamp(rawScore, 0.0, 1.0) : rawScore;

        return new InterpolationLogResult(
            log ?? new LogEntry(),
            metrics,
            normalized,
            rawScore,
            score,
            Categorize(score),
            weights);
    }

    /// <summary>Перетворення показника критичності на категорію.</summary>
    public string Categorize(double score)
    {
        if (score >= _options.CriticalThreshold) return "Critical";
        if (score >= _options.HighPriorityThreshold) return "High Priority";
        if (score >= _options.NormalThreshold) return "Normal";
        return "Low Priority";
    }
}

using System.Globalization;

namespace ATFramework2._0.Utilities.Logs;

/// <summary>
/// Вектор з 7 числових метрик логової події автоматизованого тестування.
/// Саме цей вектор x = (x₁, …, x₇) подається на вхід інтерполяційному аналізатору.
/// Перші чотири метрики відповідають вектору x = (ERROR, TimeoutException, час очікування, кількість повторів)
/// з чисельного експерименту, решта розширюють його контекстом тестового прогону.
/// </summary>
public sealed class LogMetrics
{
    /// <summary>Кількість метрик (розмірність простору ознак k).</summary>
    public const int Count = 7;

    /// <summary>Назви метрик у порядку їх розташування у векторі.</summary>
    public static readonly string[] Names =
    [
        nameof(ErrorFlag),
        nameof(TimeoutFlag),
        nameof(WaitTimeSeconds),
        nameof(RetryCount),
        nameof(ExceptionFlag),
        nameof(FailedAssertions),
        nameof(RecentContextErrors)
    ];

    /// <summary>x₁ — наявність ERROR (рівень Error або Critical): 0 або 1.</summary>
    public double ErrorFlag { get; init; }

    /// <summary>x₂ — наявність TimeoutException / таймауту: 0 або 1.</summary>
    public double TimeoutFlag { get; init; }

    /// <summary>x₃ — час очікування у секундах.</summary>
    public double WaitTimeSeconds { get; init; }

    /// <summary>x₄ — кількість повторних спроб (retry).</summary>
    public double RetryCount { get; init; }

    /// <summary>x₅ — наявність іншого винятку (exception, stacktrace, crash, fatal): 0 або 1.</summary>
    public double ExceptionFlag { get; init; }

    /// <summary>x₆ — кількість провалених перевірок (assertions) у події.</summary>
    public double FailedAssertions { get; init; }

    /// <summary>x₇ — кількість помилок у тому ж контексті за останнє часове вікно (до цієї події).</summary>
    public double RecentContextErrors { get; init; }

    /// <summary>Повертає метрики у вигляді вектора x ∈ R⁷.</summary>
    public double[] ToVector() =>
    [
        ErrorFlag,
        TimeoutFlag,
        WaitTimeSeconds,
        RetryCount,
        ExceptionFlag,
        FailedAssertions,
        RecentContextErrors
    ];

    /// <summary>Створює метрики з вектора довжини 7.</summary>
    public static LogMetrics FromVector(IReadOnlyList<double> vector)
    {
        if (vector.Count != Count)
            throw new ArgumentException($"Expected {Count} metric values, got {vector.Count}", nameof(vector));

        return new LogMetrics
        {
            ErrorFlag = vector[0],
            TimeoutFlag = vector[1],
            WaitTimeSeconds = vector[2],
            RetryCount = vector[3],
            ExceptionFlag = vector[4],
            FailedAssertions = vector[5],
            RecentContextErrors = vector[6]
        };
    }

    /// <summary>Серіалізація для зберігання у файлі логів: 7 значень через ';' (інваріантна культура).</summary>
    public string Serialize() =>
        string.Join(';', ToVector().Select(v => v.ToString("R", CultureInfo.InvariantCulture)));

    /// <summary>Десеріалізація рядка, створеного <see cref="Serialize"/>.</summary>
    public static bool TryParse(string? text, out LogMetrics? metrics)
    {
        metrics = null;
        if (string.IsNullOrWhiteSpace(text)) return false;

        var parts = text.Split(';');
        if (parts.Length != Count) return false;

        var values = new double[Count];
        for (int i = 0; i < Count; i++)
        {
            if (!double.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out values[i]))
                return false;
        }

        metrics = FromVector(values);
        return true;
    }

    public override string ToString() =>
        string.Join(", ", Names.Zip(ToVector(), (name, value) => $"{name}={value.ToString(CultureInfo.InvariantCulture)}"));
}

/// <summary>
/// Автоматичне виділення 7 метрик з тексту та рівня логу.
/// Використовується, коли метрики не передані явно при логуванні,
/// а також для логів, збережених у старому форматі (без метрик).
/// </summary>
public static class LogMetricsExtractor
{
    /// <summary>Вікно часу для підрахунку помилок у контексті (x₇).</summary>
    public static readonly TimeSpan DefaultRecentErrorsWindow = TimeSpan.FromMinutes(5);

    private static readonly Regex TimeoutRegex =
        new(@"timeout|timed\s*out", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex TimeoutExceptionRegex =
        new(@"\w*timeout\s*exception", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ExceptionRegex =
        new(@"exception|stack\s*trace|fatal|crash|unhandled|deadlock", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex DurationRegex =
        new(@"(\d+(?:[.,]\d+)?)\s*(ms|milliseconds?|s|secs?|seconds?|min|mins|minutes?)\b",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RetryNumberRegex =
        new(@"(?:retr(?:y|ies|ied|ying)|attempts?)\D{0,20}?(\d+)|(\d+)\s*(?:retries|attempts|times)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex RetryKeywordRegex =
        new(@"retr(?:y|ies|ied|ying)|re-?attempt", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex FailedAssertionRegex =
        new(@"validation\s+failed|assert(?:ion)?\s+failed", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex ExpectedButWasRegex =
        new(@"expected\b.*\bbut\s+was", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Виділяє метрики з повідомлення та рівня логу.
    /// </summary>
    /// <param name="message">Текст повідомлення.</param>
    /// <param name="level">Рівень логу.</param>
    /// <param name="recentContextErrors">Кількість недавніх помилок у тому ж контексті (x₇).</param>
    public static LogMetrics Extract(string? message, LogLevel level, double recentContextErrors = 0)
    {
        message ??= string.Empty;

        return new LogMetrics
        {
            ErrorFlag = level is LogLevel.Error or LogLevel.Critical ? 1 : 0,
            TimeoutFlag = TimeoutRegex.IsMatch(message) ? 1 : 0,
            WaitTimeSeconds = ExtractWaitTimeSeconds(message),
            RetryCount = ExtractRetryCount(message),
            ExceptionFlag = ExceptionRegex.IsMatch(TimeoutExceptionRegex.Replace(message, string.Empty)) ? 1 : 0,
            FailedAssertions = ExtractFailedAssertions(message),
            RecentContextErrors = recentContextErrors
        };
    }

    /// <summary>
    /// Рахує помилки (Error/Critical) у тому ж контексті, що сталися не пізніше за подію
    /// та не раніше, ніж за <paramref name="window"/> до неї.
    /// </summary>
    public static int CountRecentContextErrors(IEnumerable<LogEntry> previousLogs, string context, DateTime timestamp, TimeSpan? window = null)
    {
        var from = timestamp - (window ?? DefaultRecentErrorsWindow);
        return previousLogs.Count(l =>
            l.Level is LogLevel.Error or LogLevel.Critical &&
            string.Equals(l.Context, context, StringComparison.OrdinalIgnoreCase) &&
            l.Timestamp >= from && l.Timestamp <= timestamp);
    }

    /// <summary>
    /// Повертає метрики логу: збережені, якщо вони є, інакше виділені з повідомлення.
    /// </summary>
    /// <param name="log">Лог, для якого потрібні метрики.</param>
    /// <param name="previousLogs">Логи, що передують цьому (для обчислення x₇).</param>
    public static LogMetrics GetOrExtract(LogEntry log, IEnumerable<LogEntry> previousLogs)
    {
        if (log.Metrics != null) return log.Metrics;

        int recentErrors = CountRecentContextErrors(previousLogs, log.Context, log.Timestamp);
        return Extract(log.Message, log.Level, recentErrors);
    }

    private static double ExtractWaitTimeSeconds(string message)
    {
        double max = 0;
        foreach (Match match in DurationRegex.Matches(message))
        {
            var number = match.Groups[1].Value.Replace(',', '.');
            if (!double.TryParse(number, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)) continue;

            var unit = match.Groups[2].Value.ToLowerInvariant();
            double seconds = unit.StartsWith("ms") || unit.StartsWith("milli")
                ? value / 1000.0
                : unit.StartsWith("min")
                    ? value * 60.0
                    : value;

            max = Math.Max(max, seconds);
        }

        return max;
    }

    private static double ExtractFailedAssertions(string message)
    {
        int count = FailedAssertionRegex.Matches(message).Count;
        return count > 0 ? count : ExpectedButWasRegex.IsMatch(message) ? 1 : 0;
    }

    private static double ExtractRetryCount(string message)
    {
        var match = RetryNumberRegex.Match(message);
        if (match.Success)
        {
            var group = match.Groups[1].Success ? match.Groups[1] : match.Groups[2];
            if (int.TryParse(group.Value, out var count)) return count;
        }

        return RetryKeywordRegex.IsMatch(message) ? 1 : 0;
    }
}

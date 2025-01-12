namespace ATFramework2._0.Utilities.Logs;

/// <summary>
/// Клас для аналізу логів з використанням багатошарового підходу:
/// - Логістична регресія
/// - Ключові слова
/// - Динамічне вагове моделювання
/// </summary>
public class DynamicLogAnalyzer
{
    private readonly List<LogEntry> _logs;
    private readonly double[] _baseWeights = [2.0, 1.5, 3.0, 0.01]; // Ваги для фіч: Info, Warning, Error, Length
    private readonly string[] _criticalKeywords = ["exception", "timeout", "error", "failure"]; // Ключові слова для "Critical"
    private readonly string[] _warningKeywords = ["retry", "slow", "deprecated"]; // Ключові слова для "High Priority"

    /// <summary>
    /// Конструктор класу.
    /// </summary>
    /// <param name="logs">Список логів для аналізу.</param>
    public DynamicLogAnalyzer(List<LogEntry>? logs)
    {
        _logs = logs ?? [];
    }

    /// <summary>
    /// Основний метод аналізу логів.
    /// Використовує комбінацію підходів:
    /// - Аналіз ключових слів
    /// - Логістичну регресію
    /// - Динамічне вагове моделювання
    /// </summary>
    /// <returns>Список прогнозованих категорій для кожного логу.</returns>
    public List<string> AnalyzeLogs()
    {
        return _logs.Select(log =>
        {
            var features = ExtractFeatures(log); // Витягуємо фічі (основа для логістичної регресії).
            var dynamicWeights = AdjustWeightsBasedOnContext(_baseWeights); // Коригуємо ваги (динамічне моделювання).
            var score = CalculateLogScore(features, dynamicWeights); // Розраховуємо оцінку (логістична регресія).

            // Підхід: Аналіз ключових слів
            if (ContainsCriticalKeywords(log)) return "Critical"; // Якщо є критичні ключові слова.
            if (ContainsWarningKeywords(log)) return "High Priority"; // Якщо є ключові слова для попереджень.

            // Підхід: Логістична регресія
            return score > 6.0 ? "High Priority" : "Normal"; // Визначаємо пріоритет на основі оцінки.
        }).ToList();
    }

    /// <summary>
    /// Витягання фіч (основа для логістичної регресії).
    /// </summary>
    private double[] ExtractFeatures(LogEntry log)
    {
        return
        [
            log.Level == LogLevel.Info ? 1 : 0, // Інформаційні логи
            log.Level == LogLevel.Warning ? 1 : 0, // Попередження
            log.Level == LogLevel.Error ? 1 : 0, // Помилки
            log.Message.Length // Довжина повідомлення
        ];
    }

    /// <summary>
    /// Логістична регресія: розрахунок оцінки на основі фіч та ваг.
    /// </summary>
    private double CalculateLogScore(double[] features, double[] weights)
    {
        return features.Zip(weights, (feature, weight) => feature * weight).Sum();
    }

    /// <summary>
    /// Аналіз ключових слів: визначення наявності критичних ключових слів у логах.
    /// </summary>
    private bool ContainsCriticalKeywords(LogEntry log)
    {
        return _criticalKeywords.Any(keyword => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Аналіз ключових слів: визначення наявності ключових слів для попереджень у логах.
    /// </summary>
    private bool ContainsWarningKeywords(LogEntry log)
    {
        return _warningKeywords.Any(keyword => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Динамічне вагове моделювання: адаптація ваг залежно від контексту.
    /// </summary>
    /// <param name="baseWeights">Базові ваги.</param>
    /// <returns>Змінені ваги.</returns>
    private double[] AdjustWeightsBasedOnContext(double[] baseWeights)
    {
        // Загальна кількість критичних логів
        int criticalLogsCount = _logs.Count(log => log.Level == LogLevel.Error);

        // Кількість критичних логів за останні 5 хвилин
        int recentCriticalLogs = _logs.Count(log => log.Level == LogLevel.Error && log.Timestamp > DateTime.Now.AddMinutes(-5));

        // Фактор, що враховує загальну кількість критичних логів.
        double criticalFactor = 1 + Math.Log(1 + criticalLogsCount) / 10;

        // Фактор, що враховує кількість недавніх критичних логів.
        double timeFactor = 1 + (recentCriticalLogs > 5 ? 0.2 : 0);

        // Коригування ваг на основі факторів
        return baseWeights.Select(weight => weight * criticalFactor * timeFactor).ToArray();
    }
}

namespace ATFramework2._0.Utilities.Logs;

/// <summary>
/// LIPSI — Log-based Inference of Priority in Software Issues
/// Авторський підхід для класифікації логів автоматизованого тестування за пріоритетністю багів.
/// Комбінація:
/// - ключових слів;
/// - логістичної регресії;
/// - динамічного вагового моделювання;
/// - частотного аналізу;
/// - історичного ризику контексту.
/// </summary>
public class LipsiLogAnalyzer
{
    private readonly List<LogEntry> _logs;
    private readonly double[] _baseWeights = [2.0, 1.5, 3.0, 0.01]; // Info, Warning, Error, Length
    private readonly string[] _criticalKeywords = ["exception", "timeout", "fatal", "crash", "stacktrace"];
    private readonly string[] _warningKeywords = ["retry", "slow", "deprecated", "delay"];
    private readonly Dictionary<string, double> _contextRiskMap;

    public LipsiLogAnalyzer(List<LogEntry> logs)
    {
        _logs = logs ?? [];
        _contextRiskMap = CalculateContextRisks(); // Історія пріоритетів для Context
    }

    /// <summary>
    /// Основний метод LIPSI-аналізу.
    /// </summary>
    public List<string> Analyze()
    {
        return _logs.Select(log =>
        {
            // 🔹 Ключові слова: швидка класифікація
            if (ContainsKeywords(log, _criticalKeywords)) return "Critical";
            if (ContainsKeywords(log, _warningKeywords)) return "High Priority";

            // 🔹 Логістична регресія на основі фіч
            var features = ExtractFeatures(log);
            var dynamicWeights = AdjustWeights(log);
            double score = CalculateScore(features, dynamicWeights);

            // 🔹 Порогова логіка на основі сумарної оцінки
            if (score > 8.0) return "Critical";
            if (score > 6.0) return "High Priority";
            if (score > 4.0) return "Normal";
            return "Low Priority";
        }).ToList();
    }

    private bool ContainsKeywords(LogEntry log, string[] keywords)
    {
        return keywords.Any(k => log.Message.Contains(k, StringComparison.OrdinalIgnoreCase));
    }

    private double[] ExtractFeatures(LogEntry log)
    {
        return
        [
            log.Level == LogLevel.Info ? 1 : 0,
                log.Level == LogLevel.Warning ? 1 : 0,
                log.Level == LogLevel.Error ? 1 : 0,
                log.Message.Length
        ];
    }

    private double CalculateScore(double[] features, double[] weights)
    {
        return features.Zip(weights, (f, w) => f * w).Sum();
    }

    /// <summary>
    /// Динамічна модифікація ваг з урахуванням частоти помилок і контекстного ризику.
    /// </summary>
    private double[] AdjustWeights(LogEntry log)
    {
        int recentErrors = _logs.Count(l => l.Level == LogLevel.Error && l.Timestamp > DateTime.Now.AddMinutes(-5));
        double timeFactor = 1 + (recentErrors > 5 ? 0.2 : 0);

        double contextWeight = _contextRiskMap.TryGetValue(log.Context, out double risk) ? risk : 1.0;

        return _baseWeights.Select(w => w * timeFactor * contextWeight).ToArray();
    }

    /// <summary>
    /// Статистична оцінка рівня ризику контекстів (Context).
    /// </summary>
    private Dictionary<string, double> CalculateContextRisks()
    {
        var contextGroups = _logs.GroupBy(l => l.Context);
        var result = new Dictionary<string, double>();

        foreach (var group in contextGroups)
        {
            int total = group.Count();
            int errors = group.Count(g => g.Level == LogLevel.Error || g.Level == LogLevel.Critical);
            double factor = 1 + (double)errors / Math.Max(1, total); // [1, 2]
            result[group.Key] = factor;
        }

        return result;
    }
}
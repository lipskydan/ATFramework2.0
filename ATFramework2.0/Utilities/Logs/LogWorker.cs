using System.Globalization;
using ATFramework2._0.Utilities.Logs;

namespace ATFramework2._0;

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Critical
}

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
    public LogLevel Level { get; set; }
    public string Feature { get; set; } = "N/A";
    public string Context { get; set; } = "General";

    private LogMetrics? _metrics;

    /// <summary>
    /// 7 числових метрик події (див. <see cref="LogMetrics"/>), що заповнюються автоматично з логу.
    /// LogWorker та аналізатори заповнюють їх з урахуванням історії логів (x₇);
    /// до цього метрики обчислюються лише з повідомлення та рівня цього логу.
    /// </summary>
    public LogMetrics Metrics
    {
        get => _metrics ?? LogMetricsExtractor.Extract(Message, Level);
        internal set => _metrics = value;
    }

    /// <summary>Чи заповнені метрики з урахуванням історії логів.</summary>
    internal bool HasFilledMetrics => _metrics != null;
}

public class LogWorker
{
    private const string Separator = "##########################";

    public readonly List<LogEntry> _logEntries = new List<LogEntry>();
    private readonly string _logFilePath;

    public LogWorker(string logFilePath)
    {
        _logFilePath = logFilePath;

        if (File.Exists(_logFilePath))
        {
            LoadLogsFromFile();
        }
    }

    /// <summary>
    /// Додає лог. 7 метрик заповнюються автоматично з повідомлення, рівня та історії помилок у контексті.
    /// </summary>
    public void Log(string message, LogLevel level, string context = "POM", string? feature = null)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Message = message,
            Level = level,
            Feature = feature ?? "N/A",
            Context = context
        };

        _logEntries.Add(entry);
        LogMetricsExtractor.Fill(_logEntries);
        Console.WriteLine($"[Log] {entry.Timestamp} - {entry.Level} - {entry.Message}"); // Debug output
    }

    public List<LogEntry> GetAllLogs()
    {
        return _logEntries;
    }

    public List<LogEntry> GetLogsByLevel(LogLevel level)
    {
        return _logEntries.Where(log => log.Level == level).ToList();
    }

    public List<LogEntry> GetLogsByLevels(IEnumerable<LogLevel> levels)
    {
        return _logEntries.Where(log => levels.Contains(log.Level)).ToList();
    }

    public List<LogEntry> GetLogsByContext(string context)
    {
        return _logEntries.Where(log => log.Context.Equals(context, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<LogEntry> SearchLogs(string keyword)
    {
        return _logEntries
            .Where(log => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    /// <summary>
    /// Матриця метрик усіх логів (рядок — лог, стовпчик — одна з 7 метрик).
    /// </summary>
    public List<double[]> GetMetricVectors()
    {
        LogMetricsExtractor.Fill(_logEntries);
        return _logEntries.Select(log => log.Metrics.ToVector()).ToList();
    }

    /// <summary>
    /// Зберігає логи у файл у форматі:
    /// Timestamp|Level|Context|Feature|m1;m2;m3;m4;m5;m6;m7|Message
    /// </summary>
    public void SaveLogsToFile()
    {
        HashSet<DateTime> existingTimestamps = new HashSet<DateTime>();

        if (File.Exists(_logFilePath))
        {
            var lines = File.ReadAllLines(_logFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length > 0 && TryParseTimestamp(parts[0], out var timestamp))
                {
                    existingTimestamps.Add(timestamp);
                }
            }
        }

        LogMetricsExtractor.Fill(_logEntries);

        using (var writer = new StreamWriter(_logFilePath, append: true))
        {
            string lastFeature = string.Empty;
            string lastScenario = string.Empty;

            for (int i = 0; i < _logEntries.Count; i++)
            {
                var entry = _logEntries[i];
                if (existingTimestamps.Contains(entry.Timestamp))
                {
                    continue;
                }

                if (entry.Feature != lastFeature || entry.Context != lastScenario)
                {
                    if (!string.IsNullOrEmpty(lastFeature))
                    {
                        writer.WriteLine(Separator);
                    }

                    lastFeature = entry.Feature;
                    lastScenario = entry.Context;
                }

                writer.WriteLine(
                    $"{entry.Timestamp.ToString("o", CultureInfo.InvariantCulture)}|{entry.Level}|{entry.Context}|{entry.Feature}|{entry.Metrics.Serialize()}|{entry.Message}");
                existingTimestamps.Add(entry.Timestamp);
            }

            if (_logEntries.Any())
            {
                writer.WriteLine(Separator);
            }
        }

        _logEntries.Clear();
    }

    /// <summary>
    /// Завантажує логи з файлу. Підтримує новий формат (з 7 метриками)
    /// та старий формат Timestamp|Level|Context|Feature|Message — для нього метрики виділяються з повідомлення.
    /// </summary>
    private void LoadLogsFromFile()
    {
        var lines = File.ReadAllLines(_logFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length < 5) continue;
            if (!TryParseTimestamp(parts[0], out var timestamp)) continue;
            if (!Enum.TryParse<LogLevel>(parts[1], out var level)) continue;

            LogMetrics? storedMetrics = null;
            bool hasStoredMetrics = parts.Length >= 6 && LogMetrics.TryParse(parts[4], out storedMetrics);

            var entry = new LogEntry
            {
                Timestamp = timestamp,
                Level = level,
                Context = parts[2],
                Feature = parts[3],
                Message = string.Join('|', parts.Skip(hasStoredMetrics ? 5 : 4))
            };

            if (hasStoredMetrics) entry.Metrics = storedMetrics!;

            _logEntries.Add(entry);
        }

        // Логи старого формату отримують метрики автоматично.
        LogMetricsExtractor.Fill(_logEntries);
    }

    private static bool TryParseTimestamp(string text, out DateTime timestamp)
    {
        return DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.RoundtripKind, out timestamp)
               || DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out timestamp);
    }

    public void DisplayStatistics()
    {
        var groupedByLevel = _logEntries
            .GroupBy(log => log.Level)
            .Select(g => new { Level = g.Key, Count = g.Count() });

        Console.WriteLine("Log Statistics:");
        foreach (var group in groupedByLevel)
        {
            Console.WriteLine($"{group.Level}: {group.Count}");
        }

        var groupedByContext = _logEntries
            .GroupBy(log => log.Context)
            .Select(g => new { Context = g.Key, Count = g.Count() });

        Console.WriteLine("\nContext Statistics:");
        foreach (var group in groupedByContext)
        {
            Console.WriteLine($"{group.Context}: {group.Count}");
        }
    }
}

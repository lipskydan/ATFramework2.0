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
    public string Message { get; set; }
    public LogLevel Level { get; set; }
    public string Feature { get; set; } = "N/A";
    public string Context { get; set; } = "General";
}

public class LogWorker
{
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
    

    public void Log(string message, LogLevel level, string context = "POM", string feature = null)
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

    public void SaveLogsToFile()
    {
        HashSet<DateTime> existingTimestamps = new HashSet<DateTime>();

    if (File.Exists(_logFilePath))
    {
        var lines = File.ReadAllLines(_logFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length > 0 && DateTime.TryParse(parts[0], out var timestamp))
            {
                existingTimestamps.Add(timestamp);
            }
        }
    }

        using (var writer = new StreamWriter(_logFilePath, append: true))
        {
            string lastFeature = string.Empty;
            string lastScenario = string.Empty;

            foreach (var entry in _logEntries)
            {
                if (existingTimestamps.Contains(entry.Timestamp))
            {
                continue;
            }

                if (entry.Feature != lastFeature || entry.Context != lastScenario)
                {
                    if (!string.IsNullOrEmpty(lastFeature))
                    {
                        writer.WriteLine("##########################");
                    }

                    lastFeature = entry.Feature;
                    lastScenario = entry.Context;
                }

                writer.WriteLine($"{entry.Timestamp}|{entry.Level}|{entry.Context}|{entry.Feature}|{entry.Message}");
                existingTimestamps.Add(entry.Timestamp);
            }

            if (_logEntries.Any())
            {
                writer.WriteLine("##########################");
            }
        }

        _logEntries.Clear();
    }


    private void LoadLogsFromFile()
    {
        var lines = File.ReadAllLines(_logFilePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length != 5) continue;

            var entry = new LogEntry
            {
                Timestamp = DateTime.Parse(parts[0]),
                Level = Enum.Parse<LogLevel>(parts[1]),
                Context = parts[2],
                Feature = parts[3],
                Message = parts[4]
            };

            _logEntries.Add(entry);
        }
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
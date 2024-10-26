namespace ATFramework2._0;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    public string Feature { get; set; } = "N/A"; // Default if not provided
    public string Context { get; set; } = "General"; // Indicates POM or scenario context
}

public class LogWorker
{
    private readonly List<LogEntry> _logEntries = new List<LogEntry>();

    public void Log(
        string message, 
        LogLevel level, 
        string context = "POM", // Default to POM if not specified
        string feature = null, 
        string scenario = null)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Message = message,
            Level = level,
            Feature = feature ?? "N/A",  // Use default if feature is null
            Context = context
        };

        _logEntries.Add(entry);
    }

    public List<LogEntry> GetLogsByLevel(LogLevel level)
    {
        return _logEntries.Where(log => log.Level == level).ToList();
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

    public void SaveLogsToFile(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var entry in _logEntries)
            {
                writer.WriteLine($"{entry.Timestamp}|{entry.Level}|{entry.Context}|{entry.Feature}|{entry.Message}");
            }
        }
    }

    public void LoadLogsFromFile(string filePath)
    {
        if (!File.Exists(filePath)) 
            throw new FileNotFoundException($"File {filePath} not found.");

        _logEntries.Clear();

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length != 6) continue;

            var entry = new LogEntry
            {
                Timestamp = DateTime.Parse(parts[0]),
                Level = Enum.Parse<LogLevel>(parts[1]),
                Feature = parts[2],
                Context = parts[3],
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
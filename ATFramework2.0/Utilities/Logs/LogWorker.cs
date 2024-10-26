namespace ATFramework2._0;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
    public string Component { get; set; }
    public string TestCase { get; set; }
}

public class LogWorker
{
    private readonly List<LogEntry> _logEntries = new List<LogEntry>();

    public void Log(string message, LogLevel level, string component, string testCase)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Message = message,
            Level = level,
            Component = component,
            TestCase = testCase
        };

        _logEntries.Add(entry);
        Console.WriteLine($"[{entry.Timestamp}] {level}: {message}");
    }

    public List<LogEntry> GetLogsByLevel(LogLevel level)
    {
        return _logEntries.Where(log => log.Level == level).ToList();
    }

    public List<LogEntry> SearchLogs(string keyword)
    {
        return _logEntries.Where(log => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void SaveLogsToFile(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            foreach (var entry in _logEntries)
            {
                writer.WriteLine($"{entry.Timestamp}|{entry.Level}|{entry.Component}|{entry.TestCase}|{entry.Message}");
            }
        }
    }

    public void LoadLogsFromFile(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException($"File {filePath} not found.");

        _logEntries.Clear();

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length != 5) continue;

            var entry = new LogEntry
            {
                Timestamp = DateTime.Parse(parts[0]),
                Level = Enum.Parse<LogLevel>(parts[1]),
                Component = parts[2],
                TestCase = parts[3],
                Message = parts[4]
            };

            _logEntries.Add(entry);
        }
    }

    public void DisplayStatistics()
    {
        var groupedByLevel = _logEntries.GroupBy(log => log.Level)
            .Select(g => new { Level = g.Key, Count = g.Count() });

        Console.WriteLine("Log Statistics:");
        foreach (var group in groupedByLevel)
        {
            Console.WriteLine($"{group.Level}: {group.Count}");
        }
    }
}


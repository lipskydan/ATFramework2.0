using ATFramework2._0.Utilities.Logs;

namespace ATFramework2._0.Demo;

public static class LipsiAnalyzerDemo
{
    public static void Run()
    {
        var sampleLogs = new List<LogEntry>
            {
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-1), Level = LogLevel.Info, Message = "Page loaded successfully", Context = "Home", Feature = "Navigation" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-2), Level = LogLevel.Warning, Message = "Retrying after slow response", Context = "Dashboard", Feature = "Widgets" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-3), Level = LogLevel.Error, Message = "Timeout while waiting for data", Context = "DataFetch", Feature = "API" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-4), Level = LogLevel.Critical, Message = "Unhandled exception: NullReference", Context = "Login", Feature = "Auth" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-5), Level = LogLevel.Error, Message = "API returned 500 error", Context = "Profile", Feature = "API" },
                new LogEntry { Timestamp = DateTime.Now.AddMinutes(-6), Level = LogLevel.Warning, Message = "Deprecated usage in Settings", Context = "Settings", Feature = "Core" }
            };

        var analyzer = new LipsiLogAnalyzer(sampleLogs);
        var results = analyzer.Analyze();

        Console.WriteLine("====== LIPSI Log Priority Inference ======");
        for (int i = 0; i < sampleLogs.Count; i++)
        {
            var log = sampleLogs[i];
            var category = results[i];

            SetColor(category);
            Console.WriteLine($"{log.Timestamp:HH:mm:ss} | {log.Level,-8} | {log.Context,-12} | {category,-15} | {log.Message}");
        }

        Console.ResetColor();
    }

    private static void SetColor(string category)
    {
        Console.ForegroundColor = category switch
        {
            "Critical" => ConsoleColor.Red,
            "High Priority" => ConsoleColor.Yellow,
            "Normal" => ConsoleColor.White,
            "Low Priority" => ConsoleColor.DarkGray,
            _ => ConsoleColor.Gray
        };
    }
}
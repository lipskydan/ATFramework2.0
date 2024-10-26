// namespace ATFramework2._0;

// public class Program
// {
//     public static void Main(string[] args)
//     {
//         var logWorker = new LogWorker();

//         logWorker.Log("UI button not clickable", LogLevel.Error, "UI", "TestLogin");
//         logWorker.Log("Test timeout after 30 seconds", LogLevel.Warning, "Performance", "TestCheckout");
//         logWorker.Log("Unauthorized access attempt", LogLevel.Critical, "Security", "TestAuthentication");
//         logWorker.Log("Page layout is broken on mobile view", LogLevel.Warning, "UI", "ResponsiveTest");
//         logWorker.Log("Database connection timeout", LogLevel.Critical, "Backend", "TestDBConnection");
//         logWorker.Log("Unexpected null value found", LogLevel.Error, "Backend", "TestDataProcessing");
//         logWorker.Log("API returned status code 500", LogLevel.Critical, "API", "TestAPICall");
//         logWorker.Log("Memory leak detected", LogLevel.Critical, "Performance", "StressTest");
//         logWorker.Log("Deprecated method used in code", LogLevel.Warning, "CodeQuality", "CodeReviewTest");
//         logWorker.Log("SSL certificate expired", LogLevel.Error, "Security", "TestSSLCertificate");

//         var logs = logWorker.GetLogsByLevel(LogLevel.Error)
//                             .Concat(logWorker.GetLogsByLevel(LogLevel.Warning))
//                             .Concat(logWorker.GetLogsByLevel(LogLevel.Critical))
//                             .ToList();

//         var annotatedLogs = LogAnnotator.AnnotateLogs(logs);
//         Console.WriteLine("Annotated Logs:");
//         foreach (var log in annotatedLogs)
//         {
//             Console.WriteLine($"[{log.Timestamp}] {log.Level}: {log.Message}");
//             Console.WriteLine($"Component: {log.Component}, Test Case: {log.TestCase}");
//             Console.WriteLine($"Category: {log.Category}, Priority: {log.LogPriority}\n");
//         }

//         logWorker.DisplayStatistics();
//     }
// }



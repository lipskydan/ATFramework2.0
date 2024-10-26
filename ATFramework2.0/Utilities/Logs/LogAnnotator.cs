// namespace ATFramework2._0;

// public class LogAnnotator
// {
//     public static List<AnnotatedLogEntry> AnnotateLogs(List<LogEntry> logs)
//     {
//         var annotatedLogs = new List<AnnotatedLogEntry>();

//         foreach (var log in logs)
//         {
//             var category = DetermineCategory(log.Message);
//             var priority = DeterminePriority(log.Level);

//             annotatedLogs.Add(new AnnotatedLogEntry
//             {
//                 Timestamp = log.Timestamp,
//                 Message = log.Message,
//                 Level = log.Level,
//                 Component = log.Component,
//                 TestCase = log.TestCase,
//                 Category = category,
//                 LogPriority = priority
//             });
//         }

//         return annotatedLogs;
//     }

//     private static LogCategory DetermineCategory(string message)
//     {
//         if (message.Contains("UI", StringComparison.OrdinalIgnoreCase)) return LogCategory.UI;
//         if (message.Contains("timeout", StringComparison.OrdinalIgnoreCase)) return LogCategory.Performance;
//         if (message.Contains("error", StringComparison.OrdinalIgnoreCase)) return LogCategory.Functional;
//         if (message.Contains("security", StringComparison.OrdinalIgnoreCase)) return LogCategory.Security;
//         return LogCategory.Other;
//     }

//     private static Priority DeterminePriority(LogLevel level)
//     {
//         return level switch
//         {
//             LogLevel.Critical => Priority.Critical,
//             LogLevel.Error => Priority.High,
//             LogLevel.Warning => Priority.Medium,
//             _ => Priority.Low
//         };
//     }
// }


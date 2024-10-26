// namespace ATFramework2._0;

// public class LogPreprocessor
// {    
//     public static string CleanLogMessage(string message)
//     {
//         string cleanedMessage = Regex.Replace(message, @"[^\w\s]", "");
//         return cleanedMessage.Trim().ToLower();
//     }

//     public static List<LogEntry> FilterImportantLogs(List<LogEntry> logs, List<string> keywords)
//     {
//         return logs.Where(log =>
//             keywords.Any(keyword => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList();
//     }

//     public static Dictionary<string, List<LogEntry>> GroupLogsByComponent(List<LogEntry> logs)
//     {
//         return logs.GroupBy(log => log.Component)
//             .ToDictionary(group => group.Key, group => group.ToList());
//     }
// }
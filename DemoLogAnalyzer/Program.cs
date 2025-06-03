using ATFramework2._0.Demo;

class Program
{
    static void Main(string[] args)
    {
        LipsiAnalyzerDemo.Run();
        // // Ініціалізація LogWorker (шлях до файла логів або в пам'яті)
        // var logFilePath = "TestLog.txt"; // Шлях до файлу, де зберігаються логи
        // LogWorker logWorker = new LogWorker(logFilePath);

        // // Генерація 100 логів із різними рівнями критичності
        // Random random = new Random();
        // string[] contexts = { "APIWorker", "UIWorker", "DataWorker", "NetworkWorker", "BackgroundWorker" };
        // string[] features = { "LoginFeature", "ProfileFeature", "DatabaseSync", "FetchDataFeature", "ReconnectFeature" };
        // string[] messages =
        // {
        //     "Exception occurred during operation",
        //     "Deprecated function used",
        //     "Operation completed successfully",
        //     "Timeout while waiting for response",
        //     "Retrying connection to server",
        //     "Unexpected value encountered",
        //     "Resource not found",
        //     "Invalid input data",
        //     "Critical system error detected",
        //     "Operation took longer than expected"
        // };

        // for (int i = 0; i < 100; i++)
        // {
        //     LogLevel level = (LogLevel)random.Next(0, 3); // Генерація випадкового рівня логу
        //     string context = contexts[random.Next(contexts.Length)];
        //     string feature = features[random.Next(features.Length)];
        //     string message = messages[random.Next(messages.Length)];

        //     logWorker.Log(message, level, context, feature);
        // }

        // // Отримання логів для аналізу
        // List<LogEntry> logs = logWorker.GetAllLogs();

        // // Створення екземпляра DynamicLogAnalyzer
        // DynamicLogAnalyzer analyzer = new DynamicLogAnalyzer(logs);

        // // Аналіз логів
        // List<string> analysisResults = analyzer.AnalyzeLogs();

        // // Виведення результатів
        // Console.WriteLine("Аналіз логів:");
        // for (int i = 0; i < logs.Count; i++)
        // {
        //     Console.WriteLine($"Лог #{i + 1}");
        //     Console.WriteLine($"- Час: {logs[i].Timestamp}");
        //     Console.WriteLine($"- Повідомлення: {logs[i].Message}");
        //     Console.WriteLine($"- Категорія: {analysisResults[i]}");
        //     Console.WriteLine();
        // }

        // // Збереження оновлених логів після аналізу (якщо потрібно)
        // logWorker.SaveLogsToFile();
    }
}


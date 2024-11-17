using ATFramework2._0;

class Program
{
    static void Main(string[] args)
    {
        string logFilePath = "C:\\Users\\Danyil.Lipskyi\\Documents\\GitHub\\ATFramework2.0\\DemoLogs\\log.txt";

        var logWorker = new LogWorker(logFilePath);

        logWorker.Log("Application launched", LogLevel.Info, "HomePage", "Initialization");
        logWorker.Log("User successfully logged in", LogLevel.Info, "LoginPage", "User Authentication");
        logWorker.Log("Failed password entry", LogLevel.Warning, "LoginPage", "User Authentication");
        logWorker.Log("Product list retrieved", LogLevel.Info, "ProductPage", "Data Retrieval");
        logWorker.Log("Product update failed", LogLevel.Error, "ProductPage", "Data Modification");
        logWorker.Log("Temperature sensor reading high", LogLevel.Critical, "DashboardPage", "System Monitoring");
        logWorker.Log("User session ended by user", LogLevel.Info, "UserProfilePage", "Session Management");
        logWorker.Log("Session timed out", LogLevel.Warning, "HomePage", "Session Timeout");

        //logWorker.SaveLogsToFile();

        var errorLogs = logWorker.GetLogsByLevel(LogLevel.Error);
        Console.WriteLine("Error Logs:");
        foreach (var log in errorLogs)
        {
            Console.WriteLine($"{log.Timestamp} - {log.Message} [{log.Context}]");
        }

        Console.WriteLine("\nLogs from 'LoginPage':");
        var loginPageLogs = logWorker.GetLogsByContext("LoginPage");
        foreach (var log in loginPageLogs)
        {
            Console.WriteLine($"{log.Timestamp} - {log.Message} [{log.Feature}]");
        }

        Console.WriteLine("\nLogs for 'Data Retrieval' feature:");
        var dataRetrievalLogs = logWorker.SearchLogs("Data Retrieval");
        foreach (var log in dataRetrievalLogs)
        {
            Console.WriteLine($"{log.Timestamp} - {log.Message} [{log.Context}]");
        }

        logWorker.DisplayStatistics();
    }
}

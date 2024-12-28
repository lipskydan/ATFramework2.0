namespace ATFramework2._0.Verifications;

public class CheckWorker
{
    private static readonly List<string> _failures = new List<string>();
    private static readonly LogWorker _logWorker = new LogWorker("CheckWorkerLog.txt");

    public static void AddFailure(string message)
    {
        _failures.Add(message);
        _logWorker.Log($"Verification failed: {message}", LogLevel.Error, "CheckWorker", "AddFailure");
    }

    
}
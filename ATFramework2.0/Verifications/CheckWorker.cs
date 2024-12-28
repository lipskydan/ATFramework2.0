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

    public static void FinalizeChecks()
    {
        if (_failures.Count != 0)
        {
            var combinedMessage = string.Join(Environment.NewLine, _failures.Select((msg, index) => $"{index + 1}. {msg}"));
            _failures.Clear();
            throw new AssertionException($"The following checks failed:{Environment.NewLine}{combinedMessage}");
        }
    }


}
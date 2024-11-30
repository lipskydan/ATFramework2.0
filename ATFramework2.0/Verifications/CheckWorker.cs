namespace ATFramework2._0.Verifications;

public class CheckWorker
{
    private static readonly List<string> _failures = new();
    private static readonly LogWorker _logWorker = new LogWorker("CheckWorkerLog.txt");

    private static T ExecuteWithRetry<T>(Func<T> action, Func<T, bool> condition, TimeSpan? maxInterval, TimeSpan? checkInterval)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        T result = default;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                result = action();
                if (condition(result))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            result = action();
        }

        return result;
    }

    public static void Equals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: {exp}, Actual: {actualValue}";
            if (ignoreCase && typeof(T) == typeof(string))
            {
                Assert.That(actualValue as string, Is.EqualTo(exp as string).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Is.EqualTo(exp), finalMessage);
            }
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "Equals");
        }
    }

    public static void NotEquals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected value to not be: {exp}, Actual: {actualValue}";
            if (ignoreCase && typeof(T) == typeof(string))
            {
                Assert.That(actualValue as string, Is.Not.EqualTo(exp as string).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Is.Not.EqualTo(exp), finalMessage);
            }
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "NotEquals");
        }
    }

    public static void Contains(string substring, Func<string> act, bool ignoreCase = false, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: {actualValue} to contain: {substring}";
            if (ignoreCase)
            {
                Assert.That(actualValue, Does.Contain(substring).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Does.Contain(substring), finalMessage);
            }
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "Contains");
        }
    }

    public static void Matches(string pattern, Func<string> act, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: {actualValue} to match pattern: {pattern}";
            Assert.That(actualValue, Does.Match(pattern), finalMessage);
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "Matches");
        }
    }

    public static void DateTimeEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: {exp} ± {tolerance}, Actual: {actualValue}";
            Assert.That(actualValue, Is.EqualTo(exp).Within(tolerance), finalMessage);
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "DateTimeEquals");
        }
    }

    public static void DateTimeNotEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: {exp} ± {tolerance} NOT to match Actual: {actualValue}";
            Assert.That(actualValue, Is.Not.EqualTo(exp).Within(tolerance), finalMessage);
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "DateTimeNotEquals");
        }
    }


    public static void ListEquals<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? message = null)
    {
        try
        {
            var actualValue = act();
            string finalMessage = message ?? $"Expected: [{string.Join(", ", exp)}], Actual: [{string.Join(", ", actualValue)}]";
            Assert.That(actualValue, Is.EqualTo(exp), finalMessage);
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "ListEquals");
        }
    }

    public static void ListEqualsIgnoringOrder<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? message = null)
    {
        try
        {
            var actualValue = act();
            var expList = exp.OrderBy(x => x).ToList();
            var actualList = actualValue.OrderBy(x => x).ToList();
            string finalMessage = message ?? $"Expected (ignoring order): [{string.Join(", ", expList)}], Actual: [{string.Join(", ", actualList)}]";
            Assert.That(actualList, Is.EqualTo(expList), finalMessage);
        }
        catch (AssertionException ex)
        {
            _failures.Add(ex.Message);
            _logWorker.Log(ex.Message, LogLevel.Error, context: "CheckWorker", feature: "ListEqualsIgnoringOrder");
        }
    }

    public static void FinalizeChecks()
    {
        if (_failures.Any())
        {
            var combinedMessage = string.Join(Environment.NewLine, _failures.Select((msg, index) => $"{index + 1}. {msg}"));
            _failures.Clear();
            throw new AssertionException($"The following checks failed:{Environment.NewLine}{combinedMessage}");
        }
    }
}
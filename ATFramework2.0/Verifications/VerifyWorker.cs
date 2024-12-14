namespace ATFramework2._0.Verifications;

public class VerifyWorker
{
    private static readonly LogWorker _logWorker = new LogWorker("VerifyWorkerLog.txt");

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

    public static void Equals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(act, actual => ignoreCase && typeof(T) == typeof(string)
                ? string.Equals(exp as string, actual as string, StringComparison.OrdinalIgnoreCase)
                : EqualityComparer<T>.Default.Equals(exp, actual), maxInterval, checkInterval);

            string finalMessage = message ?? $"Expected: {exp}, Actual: {actualValue}";
            if (ignoreCase && typeof(T) == typeof(string))
            {
                Assert.That(actualValue as string, Is.EqualTo(exp as string).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Is.EqualTo(exp), finalMessage);
            }

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Equals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Equals");
            throw;
        }
    }

    public static void NotEquals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(act, actual => ignoreCase && typeof(T) == typeof(string)
                ? !string.Equals(exp as string, actual as string, StringComparison.OrdinalIgnoreCase)
                : !EqualityComparer<T>.Default.Equals(exp, actual), maxInterval, checkInterval);

            string finalMessage = message ?? $"Expected not equal: {exp}, Actual: {actualValue}";
            if (ignoreCase && typeof(T) == typeof(string))
            {
                Assert.That(actualValue as string, Is.Not.EqualTo(exp as string).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Is.Not.EqualTo(exp), finalMessage);
            }

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "NotEquals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "NotEquals");
            throw;
        }
    }

    public static void ListEqualsIgnoringOrder<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(act, actual => new HashSet<T>(exp).SetEquals(actual), maxInterval, checkInterval);

            var expSorted = exp.OrderBy(x => x).ToList();
            var actualSorted = actualValue.OrderBy(x => x).ToList();
            string finalMessage = message ?? $"Expected list (ignoring order): [{string.Join(", ", expSorted)}], Actual list: [{string.Join(", ", actualSorted)}]";
            Assert.That(actualSorted, Is.EqualTo(expSorted), finalMessage);

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "ListEqualsIgnoringOrder");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "ListEqualsIgnoringOrder");
            throw;
        }
    }

    public static void Contains(string substring, Func<string> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(
                act,
                actual => ignoreCase
                    ? actual.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0
                    : actual.Contains(substring),
                maxInterval,
                checkInterval
            );

            string finalMessage = message ?? $"Expected to contain: {substring}, Actual: {actualValue}";
            if (ignoreCase)
            {
                Assert.That(actualValue, Does.Contain(substring).IgnoreCase, finalMessage);
            }
            else
            {
                Assert.That(actualValue, Does.Contain(substring), finalMessage);
            }

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Contains");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Contains");
            throw;
        }
    }

    public static void Matches(string pattern, Func<string> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(
                act,
                actual => System.Text.RegularExpressions.Regex.IsMatch(actual, pattern),
                maxInterval,
                checkInterval
            );

            string finalMessage = message ?? $"Expected to match pattern: {pattern}, Actual: {actualValue}";
            Assert.That(actualValue, Does.Match(pattern), finalMessage);

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Matches");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Matches");
            throw;
        }
    }

    public static void DateTimeEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(
                act,
                actual => Math.Abs((exp - actual).Ticks) <= tolerance.Ticks,
                maxInterval,
                checkInterval
            );

            string finalMessage = message ?? $"Expected: {exp} ± {tolerance}, Actual: {actualValue}";
            Assert.That(actualValue, Is.EqualTo(exp).Within(tolerance), finalMessage);

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "DateTimeEquals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "DateTimeEquals");
            throw;
        }
    }

    public static void DateTimeNotEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(
                act,
                actual => Math.Abs((exp - actual).Ticks) > tolerance.Ticks,
                maxInterval,
                checkInterval
            );

            string finalMessage = message ?? $"Expected: {exp} ± {tolerance} NOT to match Actual: {actualValue}";
            Assert.That(actualValue, Is.Not.EqualTo(exp).Within(tolerance), finalMessage);

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "DateTimeNotEquals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "DateTimeNotEquals");
            throw;
        }
    }

    public static void ListEquals<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        try
        {
            var actualValue = ExecuteWithRetry(
                act,
                actual => EqualityComparer<IEnumerable<T>>.Default.Equals(exp, actual),
                maxInterval,
                checkInterval
            );

            string finalMessage = message ?? $"Expected list: [{string.Join(", ", exp)}], Actual list: [{string.Join(", ", actualValue)}]";
            Assert.That(actualValue, Is.EqualTo(exp), finalMessage);

            _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "ListEquals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "ListEquals");
            throw;
        }
    }


    public static void Multiple(params Action[] verifications)
    {
        var failures = new List<string>();

        foreach (var verification in verifications)
        {
            try
            {
                verification();
            }
            catch (AssertionException ex)
            {
                failures.Add(ex.Message);
                _logWorker.Log($"Verification failed in Multiple: {ex.Message}", LogLevel.Error, "VerifyWorker", "Multiple");
            }
        }

        if (failures.Any())
        {
            var summary = string.Join(Environment.NewLine, failures.Select((msg, index) => $"{index + 1}. {msg}"));
            _logWorker.Log($"Multiple verifications failed: {summary}", LogLevel.Error, "VerifyWorker", "Multiple");
            throw new AssertionException($"Multiple verifications failed:{Environment.NewLine}{summary}");
        }
    }
}
namespace ATFramework2._0.Verifications;

public class VerifyWorker
{
    public static void Equals<T>(T exp, Func<T> act, bool ignoreCase = false, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        T actualValue = default;
        
        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (ignoreCase && typeof(T) == typeof(string) && string.Equals(exp as string, actualValue as string, StringComparison.OrdinalIgnoreCase) || !ignoreCase && EqualityComparer<T>.Default.Equals(exp, actualValue))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Expected value: {exp}, Actual value: {actualValue}";
        if (ignoreCase && typeof(T) == typeof(string))
        {
            Assert.That(actualValue as string, Is.EqualTo(exp as string).IgnoreCase, message);
        }
        else
        {
            Assert.That(actualValue, Is.EqualTo(exp), message);
        }
    }

    public static void NotEquals<T>(T exp, Func<T> act, bool ignoreCase = false, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        T actualValue = default;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (ignoreCase && typeof(T) == typeof(string) && !string.Equals(exp as string, actualValue as string, StringComparison.OrdinalIgnoreCase) || !ignoreCase && !EqualityComparer<T>.Default.Equals(exp, actualValue))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Actual value should not be equal to expected value: {exp}";
        if (ignoreCase && typeof(T) == typeof(string))
        {
            Assert.That(actualValue as string, Is.Not.EqualTo(exp as string).IgnoreCase, message);
        }
        else
        {
            Assert.That(actualValue, Is.Not.EqualTo(exp), message);
        }
    }

    public static void Contains(string substring, Func<string> act, bool ignoreCase = false, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        string actualValue = string.Empty;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (ignoreCase && actualValue.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0 || !ignoreCase && actualValue.Contains(substring))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Actual value: {actualValue} should contain substring: {substring}";
        if (ignoreCase)
        {
            Assert.That(actualValue, Does.Contain(substring).IgnoreCase, message);
        }
        else
        {
            Assert.That(actualValue, Does.Contain(substring), message);
        }
    }

    public static void Matches(string pattern, Func<string> act, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        string actualValue = string.Empty;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (System.Text.RegularExpressions.Regex.IsMatch(actualValue, pattern))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Actual value: {actualValue} should match pattern: {pattern}";
        Assert.That(actualValue, Does.Match(pattern), message);
    }

    public static void DateTimeEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        DateTime actualValue = DateTime.MinValue;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (Math.Abs((exp - actualValue).Ticks) <= tolerance.Ticks)
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Expected value: {exp} within tolerance: {tolerance}, Actual value: {actualValue}";
        Assert.That(actualValue, Is.EqualTo(exp).Within(tolerance), message);
    }

    public static void DateTimeNotEquals(DateTime exp, Func<DateTime> act, TimeSpan tolerance, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        DateTime actualValue = DateTime.MinValue;

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (Math.Abs((exp - actualValue).Ticks) > tolerance.Ticks)
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Actual value should not be equal to expected value: {exp} within tolerance: {tolerance}";
        Assert.That(actualValue, Is.Not.EqualTo(exp).Within(tolerance), message);
    }

    public static void ListEquals<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        IEnumerable<T> actualValue = new List<T>();

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                if (EqualityComparer<IEnumerable<T>>.Default.Equals(exp, actualValue))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Expected list: {string.Join(", ", exp)}, Actual list: {string.Join(", ", actualValue)}";
        Assert.That(actualValue, Is.EqualTo(exp), message);
    }

    public static void ListEqualsIgnoringOrder<T>(IEnumerable<T> exp, Func<IEnumerable<T>> act, string? customMessage = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
    {
        TimeSpan maxTime = maxInterval ?? TimeSpan.Zero;
        TimeSpan interval = checkInterval ?? TimeSpan.Zero;
        IEnumerable<T> actualValue = new List<T>();

        if (maxTime != TimeSpan.Zero && interval != TimeSpan.Zero)
        {
            DateTime endTime = DateTime.Now.Add(maxTime);
            while (DateTime.Now < endTime)
            {
                actualValue = act();
                var expList = new List<T>(exp);
                var actList = new List<T>(actualValue);
                expList.Sort();
                actList.Sort();
                if (EqualityComparer<IEnumerable<T>>.Default.Equals(expList, actList))
                {
                    break;
                }
                Thread.Sleep(interval);
            }
        }
        else
        {
            actualValue = act();
        }

        string message = customMessage ?? $"Expected list (ignoring order): {string.Join(", ", exp)}, Actual list: {string.Join(", ", actualValue)}";
        Assert.That(actualValue, Is.EquivalentTo(exp), message);
    }

    public static void Multiple(params Action[] verifications)
    {
        foreach (var verification in verifications)
        {
            verification();
        }
    }
}
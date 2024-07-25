namespace ATFramework2._0.Verifications;

public class VerifyWorker
{
    public static void Equals<T>(T exp, T act, bool ignoreCase = false)
    {
        if (ignoreCase && typeof(T) == typeof(string))
        {
            Assert.That(act as string, Is.EqualTo(exp as string).IgnoreCase, $"Expected value: {exp}, Actual value: {act}");
        }
        else
        {
            Assert.That(act, Is.EqualTo(exp), $"Expected value: {exp}, Actual value: {act}");
        }
    }

    public static void NotEquals<T>(T exp, T act, bool ignoreCase = false)
    {
        if (ignoreCase && typeof(T) == typeof(string))
        {
            Assert.That(act as string, Is.Not.EqualTo(exp as string).IgnoreCase, $"Actual value should not be equal to expected value: {exp}");
        }
        else
        {
            Assert.That(act, Is.Not.EqualTo(exp), $"Actual value should not be equal to expected value: {exp}");
        }
    }

    public static void Contains(string substring, string act, bool ignoreCase = false)
    {
        if (ignoreCase)
        {
            Assert.That(act, Does.Contain(substring).IgnoreCase, $"Actual value: {act} should contain substring: {substring}");
        }
        else
        {
            Assert.That(act, Does.Contain(substring), $"Actual value: {act} should contain substring: {substring}");
        }
    }

    public static void Matches(string pattern, string act)
    {
        Assert.That(act, Does.Match(pattern), $"Actual value: {act} should match pattern: {pattern}");
    }

    public static void DateTimeEquals(DateTime exp, DateTime act, TimeSpan tolerance)
    {
        Assert.That(act, Is.EqualTo(exp).Within(tolerance), $"Expected value: {exp} within tolerance: {tolerance}, Actual value: {act}");
    }

    public static void DateTimeNotEquals(DateTime exp, DateTime act, TimeSpan tolerance)
    {
        Assert.That(act, Is.Not.EqualTo(exp).Within(tolerance), $"Actual value should not be equal to expected value: {exp} within tolerance: {tolerance}");
    }

    public static void ListEquals<T>(IEnumerable<T> exp, IEnumerable<T> act)
    {
        Assert.That(act, Is.EqualTo(exp), $"Expected list: {string.Join(", ", exp)}, Actual list: {string.Join(", ", act)}");
    }

    public static void ListEqualsIgnoringOrder<T>(IEnumerable<T> exp, IEnumerable<T> act)
    {
        Assert.That(act, Is.EquivalentTo(exp), $"Expected list (ignoring order): {string.Join(", ", exp)}, Actual list: {string.Join(", ", act)}");
    }
}

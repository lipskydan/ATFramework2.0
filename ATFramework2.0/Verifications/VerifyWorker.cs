namespace ATFramework2._0.Verifications;

public static class VerifyWorker
{
    private static readonly LogWorker _logWorker = new LogWorker("VerifyWorkerLog.txt");

    #region General Assertions
    public static void Equal<T>(T expected, T actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"{message ?? $"Expected: '{expected}'', Actual: '{actual}'"}";
        try
        {
            Assert.That(actual, Is.EqualTo(expected));
            logWorker.Log($"Verification passed : {msg}", LogLevel.Info, "VerifyWorker", "Equal");
        }
        catch
        {
            logWorker.Log($"Verification failure : Equal - {msg}", LogLevel.Error, "Verification", "Equal");
            throw;
        }
    }
    public static void NotEqual<T>(T expected, T actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{expected}', Actual: '{actual}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected));
            logWorker.Log($"Verification passed : NotEqual - {msg}", LogLevel.Info, "VerifyWorker", "NotEqual");
        }
        catch
        {
            logWorker.Log($"Verification failure : NotEqual - {msg}", LogLevel.Error, "VerifyWorker", "NotEqual");
            throw;
        }
    }
    public static void True(bool condition, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Condition: '{condition}', Message: '{message}'";
        try
        {
            Assert.IsTrue(condition);
            logWorker.Log($"Verification passed : True - {msg}", LogLevel.Info, "VerifyWorker", "True");
        }
        catch
        {
            logWorker.Log($"Verification failure : True - {msg}", LogLevel.Error, "VerifyWorker", "True");
            throw;
        }
    }
    public static void False(bool condition, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Condition: '{condition}', Message: '{message}'";
        try
        {
            Assert.IsFalse(condition);
            logWorker.Log($"Verification passed : False - {msg}", LogLevel.Info, "VerifyWorker", "False");
        }
        catch
        {
            logWorker.Log($"Verification failure : False - {msg}", LogLevel.Error, "VerifyWorker", "False");
            throw;
        }
    }
    public static void Null(object obj, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Object: '{obj}', Message: '{message}'";
        try
        {
            Assert.IsNull(obj);
            logWorker.Log($"Verification passed : Null - {msg}", LogLevel.Info, "VerifyWorker", "Null");
        }
        catch
        {
            logWorker.Log($"Verification failure : Null - {msg}", LogLevel.Error, "VerifyWorker", "Null");
            throw;
        }
    }
    public static void NotNull(object obj, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Object: '{obj}', Message: '{message}'";
        try
        {
            Assert.IsNotNull(obj);
            logWorker.Log($"Verification passed : NotNull - {msg}", LogLevel.Info, "VerifyWorker", "NotNull");
        }
        catch
        {
            logWorker.Log($"Verification failure : NotNull - {msg}", LogLevel.Error, "VerifyWorker", "NotNull");
            throw;
        }
    }
    public static void Fail(string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.Fail();
            logWorker.Log($"Verification explicitly failure : {message}", LogLevel.Error, "VerifyWorker", "Fail");
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Collection Assertions
    public static void Contains<T>(T expected, IEnumerable<T> collection, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{expected}', Collection: '{string.Join(", ", collection)}', Message: '{message}'";
        try
        {
            Assert.That(collection, Does.Contain(expected));
            logWorker.Log($"Verification passed : Contains - {msg}", LogLevel.Info, "VerifyWorker", "Contains");
        }
        catch
        {
            logWorker.Log($"Verification failure : Contains - {msg}", LogLevel.Error, "VerifyWorker", "Contains");
            throw;
        }
    }
    public static void CollectionsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{string.Join(", ", expected)}', Actual: '{string.Join(", ", actual)}', Message: '{message}'";
        try
        {
            CollectionAssert.AreEqual(expected, actual);
            logWorker.Log($"Verification passed : CollectionsEqual - {msg}", LogLevel.Info, "VerifyWorker", "CollectionsEqual");
        }
        catch
        {
            logWorker.Log($"Verification failure : CollectionsEqual - {msg}", LogLevel.Error, "VerifyWorker", "CollectionsEqual");
            throw;
        }
    }
    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{string.Join(", ", expected)}', Actual: '{string.Join(", ", actual)}', Message: '{message}'";
        try
        {
            CollectionAssert.AreEquivalent(expected, actual);
            logWorker.Log($"Verification passed : CollectionsEquivalent - {msg}", LogLevel.Info, "VerifyWorker", "CollectionsEquivalent");
        }
        catch
        {
            logWorker.Log($"Verification failure : CollectionsEquivalent - {msg}", LogLevel.Error, "VerifyWorker", "CollectionsEquivalent");
            throw;
        }
    }
    #endregion

    #region String Assertions
    public static void StringsEqual(string actual, string expected, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Expected: '{expected}', IgnoreCase: {ignoreCase}, Message: '{message}'";
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Is.EqualTo(expected.ToLower()));
            }
            else
            {
                Assert.That(actual, Is.EqualTo(expected));
            }
            logWorker.Log($"Verification passed : StringsEqual - {msg}", LogLevel.Info, "VerifyWorker", "StringsEqual");
        }
        catch
        {
            logWorker.Log($"Verification failure : StringsEqual - {msg}", LogLevel.Error, "Verification", "StringsEqual");
            throw;
        }
    }

    public static void StringContains(string actual, string substring, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $" Actual: '{actual}', Substring: '{substring}', IgnoreCase: '{ignoreCase}', Message: '{message}'";
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.Contain(substring.ToLower()));
            }
            else
            {
                Assert.That(actual, Does.Contain(substring));
            }
            logWorker.Log($"Verification passed : StringContains - {msg}", LogLevel.Info, "VerifyWorker", "StringContains");
        }
        catch
        {
            logWorker.Log($"Verification failure : StringContains - {msg}", LogLevel.Error, "VerifyWorker", "StringContains");
            throw;
        }
    }
    public static void StringStartsWith(string actual, string prefix, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Prefix: '{prefix}', IgnoreCase: '{ignoreCase}', Message: '{message}'";
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.StartWith(prefix.ToLower()));
            }
            else
            {
                Assert.That(actual, Does.StartWith(prefix));
            }
            logWorker.Log($"Verification passed : StringStartsWith - {msg}", LogLevel.Info, "VerifyWorker", "StringStartsWith");
        }
        catch
        {
            logWorker.Log($"Verification failure : StringStartsWith - {msg}", LogLevel.Error, "VerifyWorker", "StringStartsWith");
            throw;
        }
    }
    public static void StringEndsWith(string actual, string suffix, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Suffix: '{suffix}', IgnoreCase: '{ignoreCase}', Message: '{message}'";
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.EndWith(suffix.ToLower()));
            }
            else
            {
                Assert.That(actual, Does.EndWith(suffix));
            }
            logWorker.Log($"Verification passed : StringEndsWith - {msg}", LogLevel.Info, "VerifyWorker", "StringEndsWith");
        }
        catch
        {
            logWorker.Log($"Verification failure : StringEndsWith - {msg}", LogLevel.Error, "VerifyWorker", "StringEndsWith");
            throw;
        }
    }
    public static void StringMatches(string actual, string pattern, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Pattern: '{pattern}', Message: '{message}'";
        try
        {
            Assert.That(actual, Does.Match(pattern));
            logWorker.Log($"Verification passed : StringMatches - {msg}", LogLevel.Info, "VerifyWorker", "StringMatches");
        }
        catch
        {
            logWorker.Log($"Verification failure : StringMatches - {msg}", LogLevel.Error, "VerifyWorker", "StringMatches");
            throw;
        }
    }
    #endregion

    #region DateTime Assertions
    public static void DateTimeEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{expected}', Actual: '{actual}', Tolerance: '{tolerance}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
            logWorker.Log($"Verification passed : DateTimeEqual - {msg}", LogLevel.Info, "VerifyWorker", "DateTimeEqual");
        }
        catch
        {
            logWorker.Log($"Verification failure : DateTimeEqual - {msg}", LogLevel.Error, "VerifyWorker", "DateTimeEqual");
            throw;
        }
    }
    public static void DateTimeNotEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Expected: '{expected}', Actual: '{actual}', Tolerance: '{tolerance}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected).Within(tolerance));
            logWorker.Log($"Verification passed : DateTimeNotEqual - {msg}", LogLevel.Info, "VerifyWorker", "DateTimeNotEqual");
        }
        catch
        {
            logWorker.Log($"Verification failure : DateTimeNotEqual - {msg}", LogLevel.Error, "VerifyWorker", "DateTimeNotEqual");
            throw;
        }
    }
    public static void DateTimeInRange(DateTime actual, DateTime start, DateTime end, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Start: '{start}', End: '{end}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.InRange(start, end));
            logWorker.Log($"Verification passed : DateTimeInRange - {msg}", LogLevel.Info, "VerifyWorker", "DateTimeInRange");
        }
        catch
        {
            logWorker.Log($"Verification failure : DateTimeInRange - {msg}", LogLevel.Error, "VerifyWorker", "DateTimeInRange");
            throw;
        }
    }
    public static void DateTimeBefore(DateTime actual, DateTime reference, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Reference: '{reference}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.LessThan(reference));
            logWorker.Log($"Verification passed : DateTimeBefore - {msg}", LogLevel.Info, "VerifyWorker", "DateTimeBefore");
        }
        catch
        {
            logWorker.Log($"Verification failure : DateTimeBefore - {msg}", LogLevel.Error, "VerifyWorker", "DateTimeBefore");
            throw;
        }
    }
    public static void DateTimeAfter(DateTime actual, DateTime reference, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        var msg = $"Actual: '{actual}', Reference: '{reference}', Message: '{message}'";
        try
        {
            Assert.That(actual, Is.GreaterThan(reference));
            logWorker.Log($"Verification passed : DateTimeAfter - {msg}", LogLevel.Info, "VerifyWorker", "DateTimeAfter");
        }
        catch
        {
            logWorker.Log($"Verification failure : DateTimeAfter - {msg}", LogLevel.Error, "VerifyWorker", "DateTimeAfter");
            throw;
        }
    }
    #endregion
}
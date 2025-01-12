namespace ATFramework2._0.Verifications;

public class CheckWorker
{
    private static readonly List<string> _failures = new List<string>();
    private static readonly LogWorker _logWorker = new LogWorker("CheckWorkerLog.txt");

    private static void AddFailure(string message, string logFeature, LogWorker logWorker)
    {
        _failures.Add(message);
        logWorker.Log($"Validation failed: {message}", LogLevel.Error, "CheckWorker", logFeature);
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

    #region General Assertions
    public static void Equal<T>(T expected, T actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.EqualTo(expected));
            logWorker.Log($"Validation passed: {message ?? $"Expected: {expected}, Actual: {actual}"}", LogLevel.Info, "CheckWorker", "Equals");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Equal", logWorker);
        }
    }
    public static void NotEqual<T>(T expected, T actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected));
            logWorker.Log($"Validation passed: NotEqual - Expected: {expected}, Actual: {actual}, Message: {message}", LogLevel.Info, "CheckWorker", "NotEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "NotEqual", logWorker);
        }
    }
    public static void True(bool condition, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.IsTrue(condition);
            logWorker.Log($"Validation passed: True - Condition: {condition}, Message: {message}", LogLevel.Info, "CheckWorker", "True");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "True", logWorker);
        }
    }
    public static void False(bool condition, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.IsFalse(condition);
            logWorker.Log($"Validation passed: False - Condition: {condition}, Message: {message}", LogLevel.Info, "CheckWorker", "False");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "False", logWorker);
        }
    }
    public static void Null(object obj, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.IsNull(obj);
            logWorker.Log($"Validation passed: Null - Object: {obj}, Message: {message}", LogLevel.Info, "CheckWorker", "Null");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Null", logWorker);
        }
    }
    public static void NotNull(object obj, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.IsNotNull(obj);
            logWorker.Log($"Validation passed: NotNull - Object: {obj}, Message: {message}", LogLevel.Info, "CheckWorker", "NotNull");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "NotNull", logWorker);
        }
    }
    public static void Fail(string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.Fail();
            logWorker.Log($"Validation explicitly failed: {message}", LogLevel.Error, "CheckWorker", "Fail");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Fail", logWorker);
        }
    }
    #endregion

    #region Collection Assertions
    public static void Contains<T>(T expected, IEnumerable<T> collection, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(collection, Does.Contain(expected));
            logWorker.Log($"Validation passed: Contains - Expected: {expected}, Collection: {string.Join(", ", collection)}, Message: {message}", LogLevel.Info, "CheckWorker", "Contains");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains", logWorker);
        }
    }
    public static void CollectionsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            CollectionAssert.AreEqual(expected, actual);
            logWorker.Log($"Validation passed: CollectionsEqual - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "CheckWorker", "CollectionsEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains", logWorker);
        }
    }
    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            CollectionAssert.AreEquivalent(expected, actual);
            logWorker.Log($"Validation passed: CollectionsEquivalent - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "CheckWorker", "CollectionsEquivalent");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains", logWorker);
        }
    }
    #endregion

    #region String Assertions
    public static void StringsEqual(string actual, string expected, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
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
            _logWorker.Log($"Validation passed: StringsEqual - Actual: {actual}, Expected: {expected}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringsEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringsEqual", logWorker);
        }
    }
    public static void StringContains(string actual, string substring, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
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
            logWorker.Log($"Validation passed: StringContains - Actual: {actual}, Substring: {substring}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringContains");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringContains", logWorker);
        }
    }
    public static void StringStartsWith(string actual, string prefix, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
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
            logWorker.Log($"Validation passed: StringStartsWith - Actual: {actual}, Prefix: {prefix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringStartsWith");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringStartsWith", logWorker);
        }
    }
    public static void StringEndsWith(string actual, string suffix, bool ignoreCase = false, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
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
            logWorker.Log($"Validation passed: StringEndsWith - Actual: {actual}, Suffix: {suffix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringEndsWith");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringEndsWith", logWorker);
        }
    }
    public static void StringMatches(string actual, string pattern, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Does.Match(pattern));
            logWorker.Log($"Validation passed: StringMatches - Actual: {actual}, Pattern: {pattern}, Message: {message}", LogLevel.Info, "CheckWorker", "StringMatches");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringMatches", logWorker);
        }
    }
    #endregion

    #region DateTime Assertions
    public static void DateTimeEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
            logWorker.Log($"Validation passed: DateTimeEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker", logWorker);
        }
    }
    public static void DateTimeNotEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected).Within(tolerance));
            logWorker.Log($"Validation passed: DateTimeNotEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeNotEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker", logWorker);
        }
    }
    public static void DateTimeInRange(DateTime actual, DateTime start, DateTime end, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.InRange(start, end));
            logWorker.Log($"Validation passed: DateTimeInRange - Actual: {actual}, Start: {start}, End: {end}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeInRange");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker", logWorker);
        }
    }
    public static void DateTimeBefore(DateTime actual, DateTime reference, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.LessThan(reference));
            logWorker.Log($"Validation passed: DateTimeBefore - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeBefore");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker", logWorker);
        }
    }
    public static void DateTimeAfter(DateTime actual, DateTime reference, string? message = null, LogWorker? logWorker = default)
    {
        if(logWorker == default) logWorker = _logWorker;
        try
        {
            Assert.That(actual, Is.GreaterThan(reference));
            logWorker.Log($"Validation passed: DateTimeAfter - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeAfter");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker", logWorker);
        }
    }
    #endregion
}
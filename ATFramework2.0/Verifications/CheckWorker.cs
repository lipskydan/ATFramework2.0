namespace ATFramework2._0.Verifications;

public class CheckWorker
{
    private static readonly List<string> _failures = new List<string>();
    private static readonly LogWorker _logWorker = new LogWorker("CheckWorkerLog.txt");

    private static void AddFailure(string message, string logFeature)
    {
        _failures.Add(message);
        _logWorker.Log($"Validation failed: {message}", LogLevel.Error, "CheckWorker", logFeature);
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
    public static void Equal<T>(T expected, T actual, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.EqualTo(expected), message ?? $"Expected: {expected}, Actual: {actual}");
            _logWorker.Log($"Validation passed: {message ?? $"Expected: {expected}, Actual: {actual}"}", LogLevel.Info, "CheckWorker", "Equals");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Equal");
        }
    }
    public static void NotEqual<T>(T expected, T actual, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message ?? $"Expected not: {expected}, Actual: {actual}");
            _logWorker.Log($"Validation passed: NotEqual - Expected: {expected}, Actual: {actual}, Message: {message}", LogLevel.Info, "CheckWorker", "NotEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "NotEqual");
        }
    }
    public static void True(bool condition, string? message = null)
    {
        try
        {
            Assert.IsTrue(condition, message ?? $"Condition: {condition}");
            _logWorker.Log($"Validation passed: True - Condition: {condition}, Message: {message}", LogLevel.Info, "CheckWorker", "True");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "True");
        }
    }
    public static void False(bool condition, string? message = null)
    {
        try
        {
            Assert.IsFalse(condition, message ?? $"Condition: {condition}");
            _logWorker.Log($"Validation passed: False - Condition: {condition}, Message: {message}", LogLevel.Info, "CheckWorker", "False");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "False");
        }
    }
    public static void Null(object obj, string? message = null)
    {
        try
        {
            Assert.IsNull(obj, message ?? "Object should be Null");
            _logWorker.Log($"Validation passed: Null - Object: {obj}, Message: {message}", LogLevel.Info, "CheckWorker", "Null");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Null");
        }
    }
    public static void NotNull(object obj, string? message = null)
    {
        try
        {
            Assert.IsNotNull(obj, message ?? "Object should be Not Null");
            _logWorker.Log($"Validation passed: NotNull - Object: {obj}, Message: {message}", LogLevel.Info, "CheckWorker", "NotNull");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "NotNull");
        }
    }
    public static void Fail(string? message = null)
    {
        try
        {
            Assert.Fail(message ?? "Controlled fail point");
            _logWorker.Log($"Validation explicitly failed: {message}", LogLevel.Error, "CheckWorker", "Fail");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Fail");
        }
    }
    #endregion

    #region Collection Assertions
    public static void Contains<T>(T expected, IEnumerable<T> collection, string? message = null)
    {
        try
        {
            Assert.That(collection, Does.Contain(expected), message ?? $"Expected: {string.Join(", ", expected)}, Collection: {string.Join(", ", collection)}");
            _logWorker.Log($"Validation passed: Contains - Expected: {expected}, Collection: {string.Join(", ", collection)}, Message: {message}", LogLevel.Info, "CheckWorker", "Contains");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains");
        }
    }
    public static void CollectionsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null)
    {
        try
        {
            CollectionAssert.AreEqual(expected, actual, message ?? $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}");
            _logWorker.Log($"Validation passed: CollectionsEqual - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "CheckWorker", "CollectionsEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains");
        }
    }
    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null)
    {
        try
        {
            CollectionAssert.AreEquivalent(expected, actual, message ?? $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}");
            _logWorker.Log($"Validation passed: CollectionsEquivalent - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "CheckWorker", "CollectionsEquivalent");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "Contains");
        }
    }
    #endregion

    #region String Assertions
    public static void StringsEqual(string actual, string expected, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Is.EqualTo(expected.ToLower()), message ?? $"Expected: {expected}, Actual: {actual}");
            }
            else
            {
                Assert.That(actual, Is.EqualTo(expected), message ?? $"Expected: {expected}, Actual: {actual}");
            }
            _logWorker.Log($"Validation passed: StringsEqual - Actual: {actual}, Expected: {expected}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringsEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringsEqual");
        }
    }
    public static void StringContains(string actual, string substring, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.Contain(substring.ToLower()), message ?? $"Expected substring: {substring}, Actual: {actual}");
            }
            else
            {
                Assert.That(actual, Does.Contain(substring), message);
            }
            _logWorker.Log($"Validation passed: StringContains - Actual: {actual}, Substring: {substring}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringContains");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringContains");
        }
    }
    public static void StringStartsWith(string actual, string prefix, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.StartWith(prefix.ToLower()), message ?? $"Expected prefix: {prefix}, Actual: {actual}");
            }
            else
            {
                Assert.That(actual, Does.StartWith(prefix), message ?? $"Expected prefix: {prefix}, Actual: {actual}");
            }
            _logWorker.Log($"Validation passed: StringStartsWith - Actual: {actual}, Prefix: {prefix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringStartsWith");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringStartsWith");
        }
    }
    public static void StringEndsWith(string actual, string suffix, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.EndWith(suffix.ToLower()), message ?? $"Expected suffix: {suffix}, Actual: {actual}");
            }
            else
            {
                Assert.That(actual, Does.EndWith(suffix), message ?? $"Expected suffix: {suffix}, Actual: {actual}");
            }
            _logWorker.Log($"Validation passed: StringEndsWith - Actual: {actual}, Suffix: {suffix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "CheckWorker", "StringEndsWith");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringEndsWith");
        }
    }
    public static void StringMatches(string actual, string pattern, string? message = null)
    {
        try
        {
            Assert.That(actual, Does.Match(pattern), message ?? $"Expected pattern: {pattern}, Actual: {actual}");
            _logWorker.Log($"Validation passed: StringMatches - Actual: {actual}, Pattern: {pattern}, Message: {message}", LogLevel.Info, "CheckWorker", "StringMatches");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "StringMatches");
        }
    }
    #endregion

    #region DateTime Assertions
    public static void DateTimeEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance), message ?? $"Expected: {expected}, Actual: {actual}");
            _logWorker.Log($"Validation passed: DateTimeEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker");
        }
    }
    public static void DateTimeNotEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected).Within(tolerance), message ?? $"Expected: {expected}, Actual: {actual}");
            _logWorker.Log($"Validation passed: DateTimeNotEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeNotEqual");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker");
        }
    }
    public static void DateTimeInRange(DateTime actual, DateTime start, DateTime end, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.InRange(start, end), message ?? $"Expected start: {start}, Expected end: {end},  Actual: {actual}");
            _logWorker.Log($"Validation passed: DateTimeInRange - Actual: {actual}, Start: {start}, End: {end}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeInRange");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker");
        }
    }
    public static void DateTimeBefore(DateTime actual, DateTime reference, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.LessThan(reference), message ?? $"Expected reference: {reference}, Actual: {actual}");
            _logWorker.Log($"Validation passed: DateTimeBefore - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeBefore");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker");
        }
    }
    public static void DateTimeAfter(DateTime actual, DateTime reference, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.GreaterThan(reference), message ?? $"Expected reference: {reference}, Actual: {actual}");
            _logWorker.Log($"Validation passed: DateTimeAfter - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "CheckWorker", "DateTimeAfter");
        }
        catch (AssertionException ex)
        {
            AddFailure(ex.Message, "CheckWorker");
        }
    }
    #endregion
}
namespace ATFramework2._0.Verifications;

public static class VerifyWorker
{
    private static readonly LogWorker _logWorker = new LogWorker("VerifyWorkerLog.txt");

    #region General Assertions
    public static void Equal<T>(T expected, T actual, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.EqualTo(expected), message ?? $"Expected: {expected}, Actual: {actual}");
            _logWorker.Log($"Verification passed: {message ?? $"Expected: {expected}, Actual: {actual}"}", LogLevel.Info, "VerifyWorker", "Equals");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: Equal - {ex.Message}", LogLevel.Error, "Verification", "Equal");
            throw;
        }
    }

    public static void NotEqual<T>(T expected, T actual, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message ?? $"Expected not: {expected}, Actual: {actual}");
            _logWorker.Log($"Verification passed: NotEqual - Expected: {expected}, Actual: {actual}, Message: {message}", LogLevel.Info, "VerifyWorker", "NotEqual");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: NotEqual - {ex.Message}", LogLevel.Error, "VerifyWorker", "NotEqual");
            throw;
        }
    }

    public static void True(bool condition, string? message = null)
    {
        try
        {
            Assert.IsTrue(condition, message ?? $"Condition: {condition}");
            _logWorker.Log($"Verification passed: True - Condition: {condition}, Message: {message}", LogLevel.Info, "VerifyWorker", "True");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: True - {ex.Message}", LogLevel.Error, "VerifyWorker", "True");
            throw;
        }
    }

    public static void False(bool condition, string? message = null)
    {
        try
        {
            Assert.IsFalse(condition, message ?? $"Condition: {condition}");
            _logWorker.Log($"Verification passed: False - Condition: {condition}, Message: {message}", LogLevel.Info, "VerifyWorker", "False");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: False - {ex.Message}", LogLevel.Error, "VerifyWorker", "False");
            throw;
        }
    }

    public static void Null(object obj, string? message = null)
    {
        try
        {
            Assert.IsNull(obj, message ?? "Object should be Null");
            _logWorker.Log($"Verification passed: Null - Object: {obj}, Message: {message}", LogLevel.Info, "VerifyWorker", "Null");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: Null - {ex.Message}", LogLevel.Error, "VerifyWorker", "Null");
            throw;
        }
    }

    public static void NotNull(object obj, string? message = null)
    {
        try
        {
            Assert.IsNotNull(obj, message ?? "Object should be Not Null");
            _logWorker.Log($"Verification passed: NotNull - Object: {obj}, Message: {message}", LogLevel.Info, "VerifyWorker", "NotNull");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: NotNull - {ex.Message}", LogLevel.Error, "VerifyWorker", "NotNull");
            throw;
        }
    }

    public static void Fail(string? message = null)
    {
        try
        {
            Assert.Fail(message ?? "Controlled fail point");
            _logWorker.Log($"Verification explicitly failed: {message}", LogLevel.Error, "VerifyWorker", "Fail");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Fail");
            throw;
        }
    }
    #endregion

    #region Collection Assertions
    public static void Contains<T>(T expected, IEnumerable<T> collection, string? message = null)
    {
        try
        {
            Assert.That(collection, Does.Contain(expected), message ?? $"Expected: {string.Join(", ", expected)}, Collection: {string.Join(", ", collection)}");
            _logWorker.Log($"Verification passed: Contains - Expected: {expected}, Collection: {string.Join(", ", collection)}, Message: {message}", LogLevel.Info, "VerifyWorker", "Contains");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: Contains - {ex.Message}", LogLevel.Error, "VerifyWorker", "Contains");
            throw;
        }
    }

    public static void CollectionsEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null)
    {
        try
        {
            CollectionAssert.AreEqual(expected, actual, message ?? $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}");
            _logWorker.Log($"Verification passed: CollectionsEqual - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "VerifyWorker", "CollectionsEqual");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: CollectionsEqual - {ex.Message}", LogLevel.Error, "VerifyWorker", "CollectionsEqual");
            throw;
        }
    }

    public static void CollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string? message = null)
    {
        try
        {
            CollectionAssert.AreEquivalent(expected, actual, message ?? $"Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}");
            _logWorker.Log($"Verification passed: CollectionsEquivalent - Expected: {string.Join(", ", expected)}, Actual: {string.Join(", ", actual)}, Message: {message}", LogLevel.Info, "VerifyWorker", "CollectionsEquivalent");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: CollectionsEquivalent - {ex.Message}", LogLevel.Error, "VerifyWorker", "CollectionsEquivalent");
            throw;
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
            _logWorker.Log($"Verification passed: StringsEqual - Actual: {actual}, Expected: {expected}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "VerifyWorker", "StringsEqual");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Verification failed: StringsEqual - {ex.Message}", LogLevel.Error, "VerifyWorker", "StringsEqual");
            throw;
        }
    }

    public static void StringContains(string actual, string substring, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.Contain(substring.ToLower()), message);
            }
            else
            {
                Assert.That(actual, Does.Contain(substring), message);
            }
            _logWorker.Log($"Assertion passed: StringContains - Actual: {actual}, Substring: {substring}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "Verification", "StringContains");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: StringContains - {ex.Message}", LogLevel.Error, "Verification", "StringContains");
            throw;
        }
    }

    public static void StringStartsWith(string actual, string prefix, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.StartWith(prefix.ToLower()), message);
            }
            else
            {
                Assert.That(actual, Does.StartWith(prefix), message);
            }
            _logWorker.Log($"Assertion passed: StringStartsWith - Actual: {actual}, Prefix: {prefix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "Verification", "StringStartsWith");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: StringStartsWith - {ex.Message}", LogLevel.Error, "Verification", "StringStartsWith");
            throw;
        }
    }

    public static void StringEndsWith(string actual, string suffix, bool ignoreCase = false, string? message = null)
    {
        try
        {
            if (ignoreCase)
            {
                Assert.That(actual.ToLower(), Does.EndWith(suffix.ToLower()), message);
            }
            else
            {
                Assert.That(actual, Does.EndWith(suffix), message);
            }
            _logWorker.Log($"Assertion passed: StringEndsWith - Actual: {actual}, Suffix: {suffix}, IgnoreCase: {ignoreCase}, Message: {message}", LogLevel.Info, "Verification", "StringEndsWith");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: StringEndsWith - {ex.Message}", LogLevel.Error, "Verification", "StringEndsWith");
            throw;
        }
    }

    public static void StringMatches(string actual, string pattern, string? message = null)
    {
        try
        {
            Assert.That(actual, Does.Match(pattern), message);
            _logWorker.Log($"Assertion passed: StringMatches - Actual: {actual}, Pattern: {pattern}, Message: {message}", LogLevel.Info, "Verification", "StringMatches");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: StringMatches - {ex.Message}", LogLevel.Error, "Verification", "StringMatches");
            throw;
        }
    }
    #endregion

    #region DateTime Assertions
    public static void DateTimeEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance), message);
            _logWorker.Log($"Assertion passed: DateTimeEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "Verification", "DateTimeEqual");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: DateTimeEqual - {ex.Message}", LogLevel.Error, "Verification", "DateTimeEqual");
            throw;
        }
    }

    public static void DateTimeNotEqual(DateTime expected, DateTime actual, TimeSpan tolerance, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.Not.EqualTo(expected).Within(tolerance), message);
            _logWorker.Log($"Assertion passed: DateTimeNotEqual - Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}, Message: {message}", LogLevel.Info, "Verification", "DateTimeNotEqual");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: DateTimeNotEqual - {ex.Message}", LogLevel.Error, "Verification", "DateTimeNotEqual");
            throw;
        }
    }

    public static void DateTimeInRange(DateTime actual, DateTime start, DateTime end, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.InRange(start, end), message);
            _logWorker.Log($"Assertion passed: DateTimeInRange - Actual: {actual}, Start: {start}, End: {end}, Message: {message}", LogLevel.Info, "Verification", "DateTimeInRange");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: DateTimeInRange - {ex.Message}", LogLevel.Error, "Verification", "DateTimeInRange");
            throw;
        }
    }

    public static void DateTimeBefore(DateTime actual, DateTime reference, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.LessThan(reference), message);
            _logWorker.Log($"Assertion passed: DateTimeBefore - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "Verification", "DateTimeBefore");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: DateTimeBefore - {ex.Message}", LogLevel.Error, "Verification", "DateTimeBefore");
            throw;
        }
    }

    public static void DateTimeAfter(DateTime actual, DateTime reference, string? message = null)
    {
        try
        {
            Assert.That(actual, Is.GreaterThan(reference), message);
            _logWorker.Log($"Assertion passed: DateTimeAfter - Actual: {actual}, Reference: {reference}, Message: {message}", LogLevel.Info, "Verification", "DateTimeAfter");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"Assertion failed: DateTimeAfter - {ex.Message}", LogLevel.Error, "Verification", "DateTimeAfter");
            throw;
        }
    }
    #endregion

    public static void Multiple(params Action[] assertions)
    {
        try
        {
            Assert.Multiple(() =>
            {
                foreach (var assertion in assertions)
                {
                    try
                    {
                        assertion();
                    }
                    catch (AssertionException ex)
                    {
                        _logWorker.Log($"Assertion failed in Multiple: {ex.Message}", LogLevel.Error, "Verification", "Multiple");
                        throw;
                    }
                }
            });
            _logWorker.Log("All assertions in Multiple passed successfully.", LogLevel.Info, "Verification", "Multiple");
        }
        catch (AssertionException ex)
        {
            _logWorker.Log($"One or more assertions in Multiple failed: {ex.Message}", LogLevel.Error, "Verification", "Multiple");
            throw;
        }
    }
}
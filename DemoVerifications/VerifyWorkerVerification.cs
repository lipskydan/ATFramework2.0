namespace DemoVerifications;

[TestFixture]
public class VerifyWorkerVerification
{
    #region General Assertions
    [Test]
    public void TestEqual_ShouldPass_WithEqualValues()
    {
        Assert.DoesNotThrow(() => VerifyWorker.Equal(5, 5, "Values should be equal"));
    }
    [Test]
    public void TestNotEqual_ShouldPass_WithUnequalValues()
    {
        Assert.DoesNotThrow(() => VerifyWorker.NotEqual(5, 4, "Values should not be equal"));
    }
    [Test]
    public void TestTrue_ShouldPass_WhenTrue()
    {
        Assert.DoesNotThrow(() => VerifyWorker.True(true, "Condition should be true"));
    }
    [Test]
    public void TestFalse_ShouldPass_WhenFalse()
    {
        Assert.DoesNotThrow(() => VerifyWorker.False(false, "Condition should be false"));
    }
    [Test]
    public void TestNull_ShouldPass_WithNullObject()
    {
        object obj = null;
        Assert.DoesNotThrow(() => VerifyWorker.Null(obj, "Object should be null"));
    }
    [Test]
    public void TestNotNull_ShouldPass_WithNonNullObject()
    {
        object obj = new object();
        Assert.DoesNotThrow(() => VerifyWorker.NotNull(obj, "Object should not be null"));
    }
    #endregion

    #region Collection Assertions
    [Test]
    public void TestContains_ShouldPass_WhenCollectionContainsElement()
    {
        var list = new List<int> { 1, 2, 3 };
        Assert.DoesNotThrow(() => VerifyWorker.Contains(2, list, "Collection should contain the element"));
    }
    [Test]
    public void TestCollectionsEqual_ShouldPass_WhenCollectionsAreIdentical()
    {
        var expected = new List<int> { 1, 2, 3 };
        var actual = new List<int> { 1, 2, 3 };
        Assert.DoesNotThrow(() => VerifyWorker.CollectionsEqual(expected, actual, "Collections should be equal"));
    }
    [Test]
    public void CollectionsEquivalent_ShouldPass_WhenCollectionsAreEquivalent()
    {
        var expected = new List<int> { 1, 2, 3 };
        var actual = new List<int> { 3, 2, 1 };
        Assert.DoesNotThrow(() => VerifyWorker.CollectionsEquivalent(expected, actual, "Collections should be equivalent regardless of order"));
    }
    #endregion

    #region String Assertions
    [Test]
    public void StringsEqual_ShouldPass_WhenStringsAreExactlyTheSame()
    {
        string actual = "TestString";
        string expected = "TestString";
        Assert.DoesNotThrow(() => VerifyWorker.StringsEqual(actual, expected, false, "Strings should match exactly"));
    }
    [Test]
    public void StringsEqual_ShouldPass_WhenStringsAreSameIgnoringCase()
    {
        string actual = "teststring";
        string expected = "TestString";
        Assert.DoesNotThrow(() => VerifyWorker.StringsEqual(actual, expected, true, "Strings should match regardless of case"));
    }
    [Test]
    public void StringContains_ShouldPass_WhenSubstringIsPresent()
    {
        string actual = "Hello World";
        string substring = "World";
        Assert.DoesNotThrow(() => VerifyWorker.StringContains(actual, substring, false, "Substring should be found"));
    }
    [Test]
    public void StringContains_ShouldPass_WhenSubstringIsPresent_IgnoringCase()
    {
        string actual = "Hello World";
        string substring = "world";
        Assert.DoesNotThrow(() => VerifyWorker.StringContains(actual, substring, true, "Substring should be found regardless of case"));
    }
    [Test]
    public void StringStartsWith_ShouldPass_WhenPrefixIsCorrect()
    {
        string actual = "Hello World";
        string prefix = "Hello";
        Assert.DoesNotThrow(() => VerifyWorker.StringStartsWith(actual, prefix, false, "String should start with 'Hello'"));
    }
    [Test]
    public void StringStartsWith_ShouldPass_WhenPrefixIsCorrectIgnoringCase()
    {
        string actual = "hello world";
        string prefix = "Hello";
        Assert.DoesNotThrow(() => VerifyWorker.StringStartsWith(actual, prefix, true, "String should start with 'Hello' regardless of case"));
    }
    [Test]
    public void StringEndsWith_ShouldPass_WhenSuffixIsCorrect()
    {
        string actual = "Hello World";
        string suffix = "World";
        Assert.DoesNotThrow(() => VerifyWorker.StringEndsWith(actual, suffix, false, "String should end with 'World'"));
    }
    [Test]
    public void StringEndsWith_ShouldPass_WhenSuffixIsCorrectIgnoringCase()
    {
        string actual = "hello world";
        string suffix = "World";
        Assert.DoesNotThrow(() => VerifyWorker.StringEndsWith(actual, suffix, true, "String should end with 'World' regardless of case"));
    }
    [Test]
    public void StringMatches_ShouldPass_WhenPatternMatches()
    {
        string actual = "Hello World 123";
        string pattern = @"\w+\s\w+\s\d+";
        Assert.DoesNotThrow(() => VerifyWorker.StringMatches(actual, pattern, "String should match the pattern"));
    }
    [Test]
    public void StringMatches_ShouldPass_WithComplexPattern()
    {
        string actual = "Here is a sample: 24/01/2025";
        string pattern = "sample: \\d{2}/\\d{2}/\\d{4}";
        Assert.DoesNotThrow(() => VerifyWorker.StringMatches(actual, pattern, "String matches a complex date pattern"));
    }
    #endregion

    #region DateTime Assertions
    [Test]
    public void DateTimeEqual_ShouldPass_WhenDatesAreExactlyTheSame()
    {
        DateTime expected = new DateTime(2025, 1, 1);
        DateTime actual = new DateTime(2025, 1, 1);
        TimeSpan tolerance = TimeSpan.FromSeconds(1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeEqual(expected, actual, tolerance, "DateTimes should be exactly equal"));
    }
    [Test]
    public void DateTimeEqual_ShouldPass_WhenDatesAreWithinTolerance()
    {
        DateTime expected = new DateTime(2025, 1, 1, 12, 0, 0);
        DateTime actual = new DateTime(2025, 1, 1, 12, 0, 30); // 30 seconds difference
        TimeSpan tolerance = TimeSpan.FromMinutes(1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeEqual(expected, actual, tolerance, "DateTimes should be equal within one minute tolerance"));
    }
    [Test]
    public void DateTimeNotEqual_ShouldPass_WhenDatesAreClearlyDifferent()
    {
        DateTime expected = new DateTime(2025, 1, 1);
        DateTime actual = new DateTime(2025, 2, 1);
        TimeSpan tolerance = TimeSpan.FromDays(1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeNotEqual(expected, actual, tolerance, "DateTimes should not be equal as they differ by a month"));
    }
    [Test]
    public void DateTimeNotEqual_ShouldPass_WhenDatesAreOutsideTolerance()
    {
        DateTime expected = new DateTime(2025, 1, 1, 12, 0, 0);
        DateTime actual = new DateTime(2025, 1, 1, 12, 5, 0); // 5 minutes difference
        TimeSpan tolerance = TimeSpan.FromMinutes(1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeNotEqual(expected, actual, tolerance, "DateTimes should not be equal as they are outside the one minute tolerance"));
    }
    [Test]
    public void DateTimeInRange_ShouldPass_WhenDateIsExactlyAtStart()
    {
        DateTime start = new DateTime(2025, 1, 1);
        DateTime end = new DateTime(2025, 1, 31);
        DateTime actual = new DateTime(2025, 1, 1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeInRange(actual, start, end, "DateTime should be within range as it is the start date"));
    }
    [Test]
    public void DateTimeInRange_ShouldPass_WhenDateIsExactlyAtEnd()
    {
        DateTime start = new DateTime(2025, 1, 1);
        DateTime end = new DateTime(2025, 1, 31);
        DateTime actual = new DateTime(2025, 1, 31);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeInRange(actual, start, end, "DateTime should be within range as it is the end date"));
    }
    [Test]
    public void DateTimeInRange_ShouldPass_WhenDateIsWithinRange()
    {
        DateTime start = new DateTime(2025, 1, 1);
        DateTime end = new DateTime(2025, 1, 31);
        DateTime actual = new DateTime(2025, 1, 15);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeInRange(actual, start, end, "DateTime should be within range"));
    }
    [Test]
    public void DateTimeBefore_ShouldPass_WhenActualIsBeforeReference()
    {
        DateTime actual = new DateTime(2025, 1, 1);
        DateTime reference = new DateTime(2025, 1, 2);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeBefore(actual, reference, "Actual date should be before the reference date"));
    }
    [Test]
    public void DateTimeAfter_ShouldPass_WhenActualIsAfterReference()
    {
        DateTime actual = new DateTime(2025, 1, 2);
        DateTime reference = new DateTime(2025, 1, 1);
        Assert.DoesNotThrow(() => VerifyWorker.DateTimeAfter(actual, reference, "Actual date should be after the reference date"));
    }
    #endregion
}
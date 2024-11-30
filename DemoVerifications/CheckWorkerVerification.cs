namespace DemoVerifications;

[TestFixture]
public class CheckWorkerVerification
{
    [TearDown]
    public void DerivedTearDown() 
    { 
        CheckWorker.FinalizeChecks();
    }

    [Test]
    public void CheckWorker_A()
    {
        int expectedNumber = 10;
        string expectedString = "Hello World";
        DateTime expectedDate = new DateTime(2024, 1, 1);

        CheckWorker.Equals(expectedNumber, () => 12, message: "Numbers do not match"); // Fails
        CheckWorker.Equals(expectedString, () => "hello world", ignoreCase: false, message: "String comparison failed"); // Fails

        CheckWorker.Equals(42, () => 42, message: "This should pass"); // Passes
        CheckWorker.Contains("test", () => "unit testing", message: "This should pass too"); // Passes
    }

    [Test]
    public void CheckWorker_B()
    {
        DateTime expectedDate = new DateTime(2024, 1, 1);
        string expectedSubstring = "test";

        CheckWorker.DateTimeEquals(expectedDate, () => DateTime.Now, TimeSpan.FromSeconds(5), "Date does not match within tolerance"); // Fails
        CheckWorker.Contains(expectedSubstring, () => "automation tessting", message: "Substring not found"); // Fails

        CheckWorker.Equals(42, () => 42, message: "This should pass"); // Passes
        CheckWorker.Contains("test", () => "unit testing", message: "This should pass too"); // Passes
    }


    [Test]
    public void TestEqualsWithCaseSensitivity()
    {
        CheckWorker.Equals("Hello", () => "Hello");
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
        CheckWorker.Equals("Hello", () => "hello", ignoreCase: true);
    }

    [Test]
    public void TestNotEqualsWithCaseSensitivity()
    {
        CheckWorker.NotEquals("Hello", () => "World");
    }

    [Test]
    public void TestNotEqualsIgnoreCase()
    {
        CheckWorker.NotEquals("Hello", () => "world", ignoreCase: true);
    }

    [Test]
    public void TestContainsWithCaseSensitivity()
    {
        CheckWorker.Contains("lo", () => "Hello");
    }

    [Test]
    public void TestContainsIgnoreCase()
    {
        CheckWorker.Contains("lo", () => "HELLO", ignoreCase: true);
    }

    [Test]
    public void TestMatches()
    {
        CheckWorker.Matches(@"\d{3}", () => "123");
    }

    [Test]
    public void TestDateTimeEquals()
    {
        DateTime now = DateTime.Now;
        CheckWorker.DateTimeEquals(now, () => now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestDateTimeNotEquals()
    {
        DateTime now = DateTime.Now;
        CheckWorker.DateTimeNotEquals(now, () => now.AddSeconds(2), TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestListEquals()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        CheckWorker.ListEquals(expected, () => new List<int> { 1, 2, 3 });
    }

    [Test]
    public void TestListEqualsIgnoringOrder()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        CheckWorker.ListEqualsIgnoringOrder(expected, () => new List<int> { 3, 1, 2 });
    }

    // Tests with interval checks
    // [Test]
    // public void TestEqualsWithInterval()
    // {
    //     CheckWorker.Equals("Hello", () => "Hello", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestNotEqualsWithInterval()
    // {
    //     CheckWorker.NotEquals("Hello", () => "World", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestContainsWithInterval()
    // {
    //     CheckWorker.Contains("lo", () => "Hello", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestMatchesWithInterval()
    // {
    //     CheckWorker.Matches(@"\d{3}", () => "123", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestDateTimeEqualsWithInterval()
    // {
    //     DateTime now = DateTime.Now;
    //     CheckWorker.DateTimeEquals(now, () => now, TimeSpan.FromSeconds(1), maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestDateTimeNotEqualsWithInterval()
    // {
    //     DateTime now = DateTime.Now;
    //     CheckWorker.DateTimeNotEquals(now, () => now.AddSeconds(2), TimeSpan.FromSeconds(1), maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestListEqualsWithInterval()
    // {
    //     List<int> expected = new List<int> { 1, 2, 3 };
    //     CheckWorker.ListEquals(expected, () => new List<int> { 1, 2, 3 }, maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }

    // [Test]
    // public void TestListEqualsIgnoringOrderWithInterval()
    // {
    //     List<int> expected = new List<int> { 1, 2, 3 };
    //     CheckWorker.ListEqualsIgnoringOrder(expected, () => new List<int> { 3, 1, 2 }, maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    // }
}

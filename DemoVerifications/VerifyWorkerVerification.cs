namespace DemoVerifications;

[TestFixture]
public class VerifyWorkerVerification
{
    [Test]
    public void TestEqualsWithCaseSensitivity()
    {
        VerifyWorker.Equals("Hello", () => "Hello");
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
        VerifyWorker.Equals("Hello", () => "hello", ignoreCase: true);
    }

    [Test]
    public void TestNotEqualsWithCaseSensitivity()
    {
        VerifyWorker.NotEquals("Hello", () => "World");
    }

    [Test]
    public void TestNotEqualsIgnoreCase()
    {
        VerifyWorker.NotEquals("Hello", () => "world", ignoreCase: true);
    }

    [Test]
    public void TestContainsWithCaseSensitivity()
    {
        VerifyWorker.Contains("lo", () => "Hello");
    }

    [Test]
    public void TestContainsIgnoreCase()
    {
        VerifyWorker.Contains("lo", () => "HELLO", ignoreCase: true);
    }

    [Test]
    public void TestMatches()
    {
        VerifyWorker.Matches(@"\d{3}", () => "123");
    }

    [Test]
    public void TestDateTimeEquals()
    {
        DateTime now = DateTime.Now;
        VerifyWorker.DateTimeEquals(now, () => now, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestDateTimeNotEquals()
    {
        DateTime now = DateTime.Now;
        VerifyWorker.DateTimeNotEquals(now, () => now.AddSeconds(2), TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestListEquals()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        VerifyWorker.ListEquals(expected, () => new List<int> { 1, 2, 3 });
    }

    [Test]
    public void TestListEqualsIgnoringOrder()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        VerifyWorker.ListEqualsIgnoringOrder(expected, () => new List<int> { 3, 1, 2 });
    }

    [Test]
    public void TestMultiple()
    {
        VerifyWorker.Multiple(
            () => VerifyWorker.Equals("Hello", () => "Hello"),
            () => VerifyWorker.Contains("lo", () => "Hello"),
            () => VerifyWorker.ListEquals(new List<int> { 1, 2, 3 }, () => new List<int> { 1, 2, 3 })
        );
    }

    // Tests with interval checks
    [Test]
    public void TestEqualsWithInterval()
    {
        VerifyWorker.Equals("Hello", () => "Hello", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestNotEqualsWithInterval()
    {
        VerifyWorker.NotEquals("Hello", () => "World", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestContainsWithInterval()
    {
        VerifyWorker.Contains("lo", () => "Hello", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestMatchesWithInterval()
    {
        VerifyWorker.Matches(@"\d{3}", () => "123", maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestDateTimeEqualsWithInterval()
    {
        DateTime now = DateTime.Now;
        VerifyWorker.DateTimeEquals(now, () => now, TimeSpan.FromSeconds(1), maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestDateTimeNotEqualsWithInterval()
    {
        DateTime now = DateTime.Now;
        VerifyWorker.DateTimeNotEquals(now, () => now.AddSeconds(2), TimeSpan.FromSeconds(1), maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestListEqualsWithInterval()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        VerifyWorker.ListEquals(expected, () => new List<int> { 1, 2, 3 }, maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }

    [Test]
    public void TestListEqualsIgnoringOrderWithInterval()
    {
        List<int> expected = new List<int> { 1, 2, 3 };
        VerifyWorker.ListEqualsIgnoringOrder(expected, () => new List<int> { 3, 1, 2 }, maxInterval: TimeSpan.FromSeconds(5), checkInterval: TimeSpan.FromSeconds(1));
    }
}
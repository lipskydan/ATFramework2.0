namespace DemoUtilities;

[TestFixture]
public class TestDataVerifications
{
    #region General Tests
    [Test]
    public void GenerateString_ShouldReturnStringWithCustomPartAndGuid()
    {
        string customPart = "TestPart";
        string result = TestDataWorker.GenerateString(customPart);
        Assert.IsTrue(result.StartsWith(customPart));
        Assert.AreEqual(customPart.Length + 1 + 32, result.Length); // customPart + "_" + 32 chars GUID
    }

    [Test]
    public void GenerateInt_ShouldReturnPositiveInt()
    {
        string customPart = "TestInt";
        int result = TestDataWorker.GenerateInt(customPart);
        Assert.IsTrue(result >= 0);
    }

    [Test]
    public void GenerateDouble_ShouldReturnPositiveDouble()
    {
        string customPart = "TestDouble";
        double result = TestDataWorker.GenerateDouble(customPart);
        Assert.IsTrue(result >= 0.0);
        Assert.IsTrue(result < 100.0); // Since we're using a hash, it should be a small double
    }
    #endregion

    #region DateTime Tests
    [Test]
    public void GenerateCurrentDate_ShouldReturnToday()
    {
        DateTime result = TestDataWorker.GenerateCurrentDate();
        Assert.AreEqual(DateTime.Now.Date, result.Date);
    }

    [Test]
    public void GenerateRandomDate_ShouldReturnDateWithinRange()
    {
        DateTime startDate = new DateTime(2020, 1, 1);
        DateTime endDate = new DateTime(2022, 1, 1);
        DateTime result = TestDataWorker.GenerateRandomDate(startDate, endDate);
        Assert.IsTrue(result >= startDate && result <= endDate);
    }

    [Test]
    public void GenerateRandomDate_WithInvalidRange_ShouldThrowArgumentException()
    {
        DateTime startDate = new DateTime(2022, 1, 1);
        DateTime endDate = new DateTime(2020, 1, 1);
        Assert.Throws<ArgumentException>(() => TestDataWorker.GenerateRandomDate(startDate, endDate));
    }

    [Test]
    public void GenerateFormattedDate_ShouldReturnDateInSpecifiedFormat()
    {
        string format = "dd/MM/yyyy";
        string result = TestDataWorker.GenerateFormattedDate(format);
        DateTime parsedDate;
        bool isValidFormat = DateTime.TryParseExact(result, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
        Assert.IsTrue(isValidFormat);
    }
    #endregion

    #region PhoneNumber Tests
    [Test]
    public void GeneratePhoneNumber_ShouldReturnPhoneNumberWithCorrectLengthAndCountryCode()
    {
        TestDataWorker.CountryCode countryCode = TestDataWorker.CountryCode.Ukraine;
        int length = 10;
        string expectedCountryCode = "+380"; // Known code for Ukraine
        string result = TestDataWorker.GeneratePhoneNumber(countryCode, length);
        Assert.IsTrue(result.StartsWith(expectedCountryCode));
        Assert.AreEqual(expectedCountryCode.Length + length, result.Length);
    }
    #endregion

    #region Email Tests
    [Test]
    public void GenerateEmail_WithGuidInNameAndDomain_ShouldReturnEmailWithGuidInBoth()
    {
        string customPart = "user";
        TestDataWorker.EmailDomain domain = TestDataWorker.EmailDomain.Example;
        bool includeGuidInName = true;
        bool includeGuidInDomain = true;

        string result = TestDataWorker.GenerateEmail(customPart, domain, includeGuidInName, includeGuidInDomain);

        string[] parts = result.Split('@');
        string namePart = parts[0];
        string domainPart = parts[1];

        Assert.IsTrue(namePart.StartsWith(customPart));
        Assert.IsTrue(namePart.Contains("_"));
        Assert.IsTrue(domainPart.Contains("."));
    }

    [Test]
    public void GenerateEmail_WithoutGuidInName_ShouldReturnEmailWithCustomPartOnly()
    {
        string customPart = "user";
        TestDataWorker.EmailDomain domain = TestDataWorker.EmailDomain.Example;
        bool includeGuidInName = false;
        bool includeGuidInDomain = false;

        string result = TestDataWorker.GenerateEmail(customPart, domain, includeGuidInName, includeGuidInDomain);

        string[] parts = result.Split('@');
        string namePart = parts[0];
        string domainPart = parts[1];
        Assert.AreEqual(customPart, namePart);
        Assert.AreEqual("example.com", domainPart);
    }

    [Test]
    public void GenerateEmail_WithGuidInDomain_ShouldReturnEmailWithGuidInDomain()
    {
        string customPart = "user";
        TestDataWorker.EmailDomain domain = TestDataWorker.EmailDomain.Example;
        bool includeGuidInName = false;
        bool includeGuidInDomain = true;

        string result = TestDataWorker.GenerateEmail(customPart, domain, includeGuidInName, includeGuidInDomain);

        string[] parts = result.Split('@');
        string domainPart = parts[1];
        Assert.IsTrue(domainPart.Contains("."));
        Assert.IsTrue(domainPart.Length > "example.com".Length);
    }
    #endregion
}
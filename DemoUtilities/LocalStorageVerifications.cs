namespace DemoUtilities;

public class LocalStorageVerifications
{
    IWebDriver driver;
    LocalStorageWorker localStorage;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var chromeOptions = new ChromeOptions
        {
            BrowserVersion = "122.0.6261.94"
        };
        driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/");

        localStorage = new LocalStorageWorker(driver);
    }

    [SetUp]
    public void Setup()
    {
        localStorage.AddToLocalStorage(key: "key1", value: "value1");
        localStorage.AddToLocalStorage(key: "key2", value: "value2");
        localStorage.AddToLocalStorage(key: "key3", value: "value3");
    }

    [TearDown]
    public void TearDown()
    {
        localStorage.ClearLocalStorage();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        driver.Quit();
    }

    [Test]
    public void VerifyGetLocalStorage()
    {
        var actDict = localStorage.GetLocalStorage();
        var expDict = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" }
        };

        CollectionAssert.AreEqual(expDict, actDict, "The dictionaries are not equal.");
    }

    [Test]
    public void VerifyAddToLocalStorage()
    {
        localStorage.AddToLocalStorage(key: "key4", value: "value4");
        var actDict = localStorage.GetLocalStorage();
        var expDict = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" },
            { "key4", "value4" }
        };

        CollectionAssert.AreEqual(expDict, actDict, "The dictionaries are not equal.");
    }

    [Test]
    public void VerifyUpdateLocalStorageValue()
    {
        localStorage.UpdateLocalStorageValue(key: "key1", newValue: "value1_new");
        var actDict = localStorage.GetLocalStorage();
        var expDict = new Dictionary<string, string>
        {
            { "key1", "value1_new" },
            { "key2", "value2"     },
            { "key3", "value3"     }
        };

        CollectionAssert.AreEqual(expDict, actDict, "The dictionaries are not equal.");
    }

    [Test]
    public void VerifyDeleteFromLocalStorage()
    {
        localStorage.DeleteFromLocalStorage(key: "key2");
        var actDict = localStorage.GetLocalStorage();
        var expDict = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key3", "value3" }
        };

        CollectionAssert.AreEqual(expDict, actDict, "The dictionaries are not equal.");
    }

    [Test]
    public void VerifyCleanLocalStorage()
    {
        localStorage.ClearLocalStorage();
        var actDict = localStorage.GetLocalStorage();
        var expDict = new Dictionary<string, string> {};

        CollectionAssert.AreEqual(expDict, actDict, "The dictionaries are not equal.");
    }
}
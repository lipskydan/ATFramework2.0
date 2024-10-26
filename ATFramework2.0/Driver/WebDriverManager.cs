namespace ATFramework2._0.Driver;

public class WebDriverManager : IWebDriverManager, IDisposable
{
    private readonly TestSettings _testSettings;
    public IWebDriver Driver { get; }
    public Lazy<WebDriverWait> WebDriverWait { get; }
    public ElementFinder ElementFinder { get; }
    public ElementsFinder ElementsFinder { get; }
    public LogWorker LogWorker { get; set;  }

    public WebDriverManager(TestSettings testSettings)
    {
        _testSettings = testSettings;
        LogWorker = new LogWorker();
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        WebDriverWait = new Lazy<WebDriverWait>(GetWaitDriver);
        ElementFinder = new ElementFinder(this);
    }

    private IWebDriver GetWebDriver()
    {
        switch (_testSettings.BrowserType)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions
                {
                    BrowserVersion = _testSettings.BrowserVersion
                };
                return new ChromeDriver(chromeOptions);

            case BrowserType.Firefox:
                var firefoxOptions = new FirefoxOptions
                {
                    BrowserVersion = _testSettings.BrowserVersion
                };
                return new FirefoxDriver(firefoxOptions);

            case BrowserType.Safari:
                var safariOptions = new SafariOptions();
                return new SafariDriver(safariOptions);

            default:
                throw new ArgumentException("Unsupported browser type: " + _testSettings.BrowserType);
        }
    }  

    private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions()),
            BrowserType.Firefox => new RemoteWebDriver(_testSettings.GridUri, new FirefoxOptions()),
            BrowserType.Safari => new RemoteWebDriver(_testSettings.GridUri, new SafariOptions()),
            _ => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions())
        };
    }

    private WebDriverWait GetWaitDriver()
    {
        return new(Driver, timeout: TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 30))
        {
            PollingInterval = TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 5)
        };
    }

    public void OpenApplicationStartPage()
    {
        Driver.Manage().Window.Maximize();
        Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
    }

    public void Dispose()
    {
        Driver.Quit();
        LogWorker.SaveLogsToFile(_testSettings.PathToSaveReport + $"logs_{DateTime.Now}.txt");
    }
}

public enum BrowserType
{
    Chrome,
    Firefox,
    Safari,
}

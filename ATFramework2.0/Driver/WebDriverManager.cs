namespace ATFramework2._0.Driver;

public class WebDriverManager : IWebDriverManager, IDisposable
{
    private readonly TestSettings _testSettings;
    public IWebDriver Driver { get; }
    public Lazy<WebDriverWait> WebDriverWait { get; }
    public ElementFinder ElementFinder { get; }
    public ElementsFinder ElementsFinder { get; }
    public LogWorker LogWorker { get; }

    public WebDriverManager(TestSettings testSettings)
    {
        _testSettings = testSettings;

        // Initialize a single log file for all tests
        //LogWorker.SaveLogsToFile(_testSettings.Logs.PathToSave + $"logs_{DateTime.Now:MM_dd_yyyy_HH_mm_ss}.txt"); 
        // string logFilePath = Path.Combine(
        //     _testSettings.Report.PathToSave,
        //     "TestLogs.txt"
        // );
        
        string logFileName = $"GlobalTestLogs_{TestRunContext.TestRunTimestamp}.txt";
        string logFilePath = Path.Combine(_testSettings.Report.PathToSave, logFileName);

        LogWorker = new LogWorker(logFilePath);

        Driver = _testSettings.Utilities.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        WebDriverWait = new Lazy<WebDriverWait>(GetWaitDriver);
        ElementFinder = new ElementFinder(this);
    }

    private IWebDriver GetWebDriver()
    {
        switch (_testSettings.Browser.Type)
        {
            case BrowserType.Chrome:
                var chromeOptions = new ChromeOptions
                {
                    BrowserVersion = _testSettings.Browser.Version
                };
                return new ChromeDriver(chromeOptions);

            case BrowserType.Firefox:
                var firefoxOptions = new FirefoxOptions
                {
                    BrowserVersion = _testSettings.Browser.Version
                };
                return new FirefoxDriver(firefoxOptions);

            case BrowserType.Safari:
                var safariOptions = new SafariOptions();
                return new SafariDriver(safariOptions);

            default:
                throw new ArgumentException("Unsupported browser type: " + _testSettings.Browser.Type);
        }
    }

    private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.Browser.Type switch
        {
            BrowserType.Chrome => new RemoteWebDriver(_testSettings.Utilities.GridUri, new ChromeOptions()),
            BrowserType.Firefox => new RemoteWebDriver(_testSettings.Utilities.GridUri, new FirefoxOptions()),
            BrowserType.Safari => new RemoteWebDriver(_testSettings.Utilities.GridUri, new SafariOptions()),
            _ => new RemoteWebDriver(_testSettings.Utilities.GridUri, new ChromeOptions())
        };
    }

    private WebDriverWait GetWaitDriver()
    {
        return new(Driver, timeout: TimeSpan.FromSeconds(_testSettings.Utilities.TimeoutInterval ?? 30))
        {
            PollingInterval = TimeSpan.FromSeconds(_testSettings.Utilities.TimeoutInterval ?? 5)
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
        LogWorker.SaveLogsToFile();
    }
}
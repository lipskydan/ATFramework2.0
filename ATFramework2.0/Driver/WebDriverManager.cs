namespace ATFramework2._0.Driver;

public class WebDriverManager : IWebDriverManager, IDisposable
{
    private readonly TestSettings _testSettings;
    private readonly Lazy<WebDriverWait> _webDriverWait;
    
    public IWebDriver Driver { get; }
    public WebElementFinder ElementFinder { get; }
    
    public WebDriverManager(TestSettings testSettings)
    {
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        _webDriverWait = new Lazy<WebDriverWait>(GetWaitDriver);
        ElementFinder = new WebElementFinder(this);
    }
    private IWebDriver GetWebDriver()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        // return new ChromeDriver();
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            BrowserType.Safari => new SafariDriver(),
            _ => new ChromeDriver()
        };
    }
    private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions()),
            BrowserType.Firefox =>  new RemoteWebDriver(_testSettings.GridUri, new FirefoxOptions()),
            BrowserType.Safari =>  new RemoteWebDriver(_testSettings.GridUri, new SafariOptions()),
            _ =>  new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions())
        };
    }
    
    private WebDriverWait GetWaitDriver()
    {
        return new(Driver, timeout: TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 30))
        {
            PollingInterval = TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 1)
        };
    }

    public void OpenApplicationStartPage()
    {
        Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
    }

    public void Dispose()
    {
       Driver.Quit();
    }
    
    public IWebElement FindElement(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => Driver.FindElement(elementLocator));
    }

    public IEnumerable<IWebElement> FindElements(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => Driver.FindElements(elementLocator));
    }
    
}


public enum BrowserType
{
    Chrome,
    Firefox,
    Safari,
}

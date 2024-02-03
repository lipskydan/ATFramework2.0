using System.Reflection;
using ATFramework2._0.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ATFramework2._0.Driver;

public class WebWebDriverManager : IWebDriverManager, IDisposable
{
    private readonly TestSettings _testSettings;
    private readonly Lazy<WebDriverWait> _webDriverWait;
    
    public IWebDriver Driver { get; }
    
    public WebWebDriverManager(TestSettings testSettings)
    {
        _testSettings = testSettings;
        Driver = _testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
        // Driver.Navigate().GoToUrl("https://anupdamoda.github.io/AceOnlineShoePortal/");
        _webDriverWait = new Lazy<WebDriverWait>(GetWaitDriver);
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
    
    public string TakeScreenshotAsPath(string fileName)
    {
        var screenshot = Driver.TakeScreenshot();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}//{fileName}.png";
        screenshot.SaveAsFile(path);
        return path;
    }
    
    public IWebElement FindElement(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => Driver.FindElement(elementLocator));
    }

    public IEnumerable<IWebElement> FindElements(By elementLocator)
    {
        return _webDriverWait.Value.Until(_ => Driver.FindElements(elementLocator));
    }

    private WebDriverWait GetWaitDriver()
    {
        return new(Driver, timeout: TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 30))
        {
            PollingInterval = TimeSpan.FromSeconds(_testSettings.TimeoutInterval ?? 1)
        };
    }

    public void Dispose()
    {
       Driver.Quit();
    }
}


public enum BrowserType
{
    Chrome,
    Firefox,
    Safari,
    EdgeChromium
}

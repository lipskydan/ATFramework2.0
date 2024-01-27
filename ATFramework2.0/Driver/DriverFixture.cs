using System.Configuration;
using System.Reflection;
using ATFramework2._0.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.Extensions;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ATFramework2._0.Driver;

public class DriverFixture : IDriverFixture, IDisposable
{
    //private readonly TestSettings _testSettings;

    public IWebDriver Driver { get; }
    
    public DriverFixture(/**TestSettings testSettings*/)
    {
        //_testSettings = testSettings;
        Driver = GetWebDriver(); //_testSettings.TestRunType == TestRunType.Local ? GetWebDriver() : GetRemoteWebDriver();
        //Driver.Navigate().GoToUrl(_testSettings.ApplicationUrl);
        
        Driver.Navigate().GoToUrl("https://anupdamoda.github.io/AceOnlineShoePortal/");
    }


    private IWebDriver GetWebDriver()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        return new ChromeDriver();
        /*return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            BrowserType.Safari => new SafariDriver(),
            _ => new ChromeDriver()
        };*/
    }
    
    /*private IWebDriver GetRemoteWebDriver()
    {
        return _testSettings.BrowserType switch
        {
            BrowserType.Chrome => new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions()),
            BrowserType.Firefox =>  new RemoteWebDriver(_testSettings.GridUri, new FirefoxOptions()),
            BrowserType.Safari =>  new RemoteWebDriver(_testSettings.GridUri, new SafariOptions()),
            _ =>  new RemoteWebDriver(_testSettings.GridUri, new ChromeOptions())
        };
    }*/
    
    public string TakeScreenshotAsPath(string fileName)
    {
        var screenshot = Driver.TakeScreenshot();
        var path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}//{fileName}.png";
        screenshot.SaveAsFile(path);
        return path;
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

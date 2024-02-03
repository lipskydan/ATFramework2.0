using OpenQA.Selenium;

namespace ATFramework2._0.Driver;

public interface IWebDriverManager
{
    IWebDriver Driver { get; }

    string TakeScreenshotAsPath(string fileName);
    
    IWebElement FindElement(By elementLocator);
    IEnumerable<IWebElement> FindElements(By elementLocator);
}
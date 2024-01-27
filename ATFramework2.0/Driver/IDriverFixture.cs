using OpenQA.Selenium;

namespace ATFramework2._0.Driver;

public interface IDriverFixture
{
    IWebDriver Driver { get; }

    string TakeScreenshotAsPath(string fileName);
}
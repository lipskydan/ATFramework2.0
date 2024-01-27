using OpenQA.Selenium;

namespace ATFramework2._0.Driver;

public interface IDriverWait
{
    IWebElement FindElement(By elementLocator);
    IEnumerable<IWebElement> FindElements(By elementLocator);
}
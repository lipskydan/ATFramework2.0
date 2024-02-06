namespace ATFramework2._0.Driver;

public interface IWebDriverManager
{
    IWebDriver Driver { get; }
    WebElementFinder ElementFinder { get; }
    WebElementsFinder ElementsFinder { get; }
    void OpenApplicationStartPage();
    IWebElement FindElement(By elementLocator);
    IEnumerable<IWebElement> FindElements(By elementLocator);
}
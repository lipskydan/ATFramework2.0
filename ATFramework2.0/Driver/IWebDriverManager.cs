namespace ATFramework2._0.Driver;

public interface IWebDriverManager
{
    IWebDriver Driver { get; }
    Lazy<WebDriverWait> WebDriverWait {get; }
    ElementFinder ElementFinder { get; }
    ElementsFinder ElementsFinder { get; }
    void OpenApplicationStartPage();
}
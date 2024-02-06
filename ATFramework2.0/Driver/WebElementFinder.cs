namespace ATFramework2._0.Driver;

public class WebElementFinder
{
    private readonly IWebDriverManager _driverManager;

    public WebElementFinder(IWebDriverManager driverManager)
    {
        _driverManager = driverManager;
    }

    public IWebElement Css(string cssSelector) => _driverManager.FindElement(By.CssSelector(cssSelector));
    public IWebElement XPath(string xpath) => _driverManager.FindElement(By.XPath(xpath));
    public IWebElement Id(string id) => _driverManager.FindElement(By.Id(id));
    public IWebElement LinkText(string text) => _driverManager.FindElement(By.LinkText(text));
}
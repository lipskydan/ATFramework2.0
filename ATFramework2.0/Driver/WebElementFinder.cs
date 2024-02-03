namespace ATFramework2._0.Driver;

public class WebElementFinder
{
    private readonly IWebDriverManager _driverManager;

    public WebElementFinder(IWebDriverManager driverManager)
    {
        _driverManager = driverManager;
    }
    
    public IWebElement Css(string cssSelector)
    {
        return _driverManager.FindElement(By.CssSelector(cssSelector));
    }
    public IWebElement XPath(string xpath)
    {
        return _driverManager.FindElement(By.XPath(xpath));
    }
}
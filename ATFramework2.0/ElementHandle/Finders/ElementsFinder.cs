namespace ATFramework2._0.ElementHandle.Finders;

public class ElementsFinder
{
    private readonly IWebDriverManager _driverManager;

    public ElementsFinder(IWebDriverManager driverManager)
    {
        _driverManager = driverManager;
    }
    
    public IEnumerable<IWebElement> Css(string cssSelector) => _driverManager.FindElements(By.CssSelector(cssSelector));
    public IEnumerable<IWebElement> XPath(string xpath) => _driverManager.FindElements(By.XPath(xpath));
    public IEnumerable<IWebElement> Id(string id) => _driverManager.FindElements(By.Id(id));
}
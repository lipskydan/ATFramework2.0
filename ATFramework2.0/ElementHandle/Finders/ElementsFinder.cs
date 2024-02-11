namespace ATFramework2._0.ElementHandle.Finders;

public class ElementsFinder
{
    private readonly IWebDriverManager _driverManager;

    public ElementsFinder(IWebDriverManager driverManager)
    {
        _driverManager = driverManager;
    }
    
    public Elements Css(string cssSelector) => new(_driverManager.FindElements(By.CssSelector(cssSelector)));
    public Elements XPath(string xpath) => new(_driverManager.FindElements(By.XPath(xpath)));
    public Elements Id(string id) => new(_driverManager.FindElements(By.Id(id)));
}
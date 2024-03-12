namespace ATFramework2._0.ElementHandle.Finders;

public class ElementsFinder
{
    private readonly IWebDriverManager _driverManager;

    public ElementsFinder(IWebDriverManager driverManager) => _driverManager = driverManager;

    public Elements Css(string cssSelector) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElements(By.CssSelector(cssSelector))));
    public Elements XPath(string xpath) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElements(By.XPath(xpath))));
    public Elements Id(string id) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElements(By.Id(id))));
}
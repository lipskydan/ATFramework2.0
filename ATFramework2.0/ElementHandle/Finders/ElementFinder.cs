namespace ATFramework2._0.ElementHandle.Finders;

public class ElementFinder
{
    private readonly IWebDriverManager _driverManager;

    public ElementFinder(IWebDriverManager driverManager) => _driverManager = driverManager;

    public Element Css(string cssSelector) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElement(By.CssSelector(cssSelector))));
    public Element XPath(string xpath) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElement(By.XPath(xpath))));
    public Element Id(string id) => new(_driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElement(By.Id(id))));

    //public Element LinkText(string text) => new(_driverManager.FindElement(By.LinkText(text)));
    //TODO: fix _Id (now use for SelectElement)
    public IWebElement _Id(string id) => _driverManager.WebDriverWait.Value.Until(_ => _driverManager.Driver.FindElement(By.Id(id)));
}
namespace ATFramework2._0.ElementHandle;

public static class Wait
{
    private static IWebDriver _driver;

    // Method to initialize the WebDriver
    public static void Initialize(IWebDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
    }

    // Method: UntilTrue - Waits until a custom condition is true
    public static void UntilTrue(Func<IWebDriver, bool> condition, int timeoutInSeconds, string timeoutMessage = "Condition not met within timeout.")
    {
        if (_driver == null) throw new InvalidOperationException("Wait class is not initialized. Call Wait.Initialize() first.");

        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

        try
        {
            wait.Until(condition);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception(timeoutMessage);
        }
    }

    // Method: UntilElementIsVisible
    public static Element UntilElementIsVisible(By locator, int timeoutInSeconds)
    {
        if (_driver == null) throw new InvalidOperationException("Wait class is not initialized. Call Wait.Initialize() first.");

        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
        return new Element(element);
    }

    // Method: UntilElementIsClickable
    public static Element UntilElementIsClickable(By locator, int timeoutInSeconds)
    {
        if (_driver == null) throw new InvalidOperationException("Wait class is not initialized. Call Wait.Initialize() first.");

        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        return new Element(element);
    }

    // Method: UntilAttributeContains
    public static bool UntilAttributeContains(Element element, string attribute, string value, int timeoutInSeconds)
    {
        if (_driver == null) throw new InvalidOperationException("Wait class is not initialized. Call Wait.Initialize() first.");

        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(driver =>
        {
            string attributeValue = element.webElementCore.GetAttribute(attribute);
            return attributeValue != null && attributeValue.Contains(value);
        });
    }
}

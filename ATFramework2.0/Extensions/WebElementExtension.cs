namespace ATFramework2._0.Extensions;

public static class WebElementExtension
{
    private static IWebDriver GetDriverFromElement(IWebElement element)
    {
        IWebDriver? driver = null;
        var wrappedElement = element as IWrapsDriver;

        if (wrappedElement == null)
        {
            PropertyInfo pi = element.GetType()
                .GetProperty("WrappedDriver", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pi != null)
            {
                driver = pi.GetValue(element, null) as IWebDriver;
            }
        }
        else
        {
            driver = wrappedElement.WrappedDriver;
        }

        return driver;
    }
    private static string BackupElementStyle(this IWebElement element) => element.GetAttribute("style");
    private static void ResetElementStyle(this IWebElement element, string originalStyle)
    {
        IWebDriver driver = GetDriverFromElement(element)
            ?? throw new InvalidOperationException("Unable to retrieve driver from element");
        
        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
        string script = "arguments[0].setAttribute('style', arguments[1]);";
        jsExecutor.ExecuteScript(script, element, originalStyle);
    }
    private static void Highlight(this IWebElement element, int highlightDuration = 2000)
    {
        IWebDriver driver = GetDriverFromElement(element) 
            ?? throw new InvalidOperationException("Unable to retrieve driver from element");
        
        string script = "arguments[0].setAttribute('style', arguments[1]);";
        IJavaScriptExecutor? jsExecutor = driver as IJavaScriptExecutor;
        jsExecutor?.ExecuteScript(script, element, "border: 4px solid red;");
        
        Thread.Sleep(highlightDuration);
    }
    public static void PerformActionWithHighlighting(this IWebElement element, Action action)
    {
        var originalStyle = element.BackupElementStyle();
        element.Highlight();
        element.ResetElementStyle(originalStyle);
        action();
    }
    public static void ScrollToElement(this IWebElement element)
    {
        IWebDriver driver = GetDriverFromElement(element)
        ?? throw new InvalidOperationException("Unable to retrieve driver from element");
        
        var jsExecutor = (IJavaScriptExecutor)driver;
        jsExecutor.ExecuteScript(@"
        var element = arguments[0];
        var elementRect = element.getBoundingClientRect();
        var absoluteElementTop = elementRect.top + window.pageYOffset;
        var middle = absoluteElementTop - (window.innerHeight * 0.4); // Adjust 0.4 to position slightly above center
        window.scrollTo(0, middle);
        ", element);
    }
}
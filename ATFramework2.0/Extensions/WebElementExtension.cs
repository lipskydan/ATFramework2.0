using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ATFramework2._0.Extensions;

public static class WebElementExtension
{
    public static void SelectDropDownByText(this IWebElement element, string text)
    {
        var select = new SelectElement(element);
        select.SelectByText(text);
    }

    public static void SelectDropDownByValue(this IWebElement element, string value)
    {
        var select = new SelectElement(element);
        select.SelectByValue(value);
    }

    public static void SelectDropDownByIndex(this IWebElement element, int index)
    {
        var select = new SelectElement(element);
        select.SelectByIndex(index);
    }
    
    public static void ClearAndEnterText(this IWebElement element, string value)
    {
        element.Clear();
        element.SendKeys(value);
    }
}
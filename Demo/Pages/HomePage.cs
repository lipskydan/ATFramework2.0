using ATFramework2._0.Driver;
using OpenQA.Selenium;

namespace Demo.Pages;

public interface IHomePage
{
    void click_SignInPortal();
}

class HomePage : IHomePage
{
    private readonly IWebDriverManager _webDriver;

    public HomePage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }
    
    public IWebElement menu_input => _webDriver.FindElement(By.CssSelector("#menuToggle > input[type=checkbox]"));
    public IWebElement lnkSignInPortal => _webDriver.FindElement(By.LinkText("Sign In Portal"));
    
    public void click_SignInPortal()
    {
        menu_input.Click();
        lnkSignInPortal.Click();
    }
}





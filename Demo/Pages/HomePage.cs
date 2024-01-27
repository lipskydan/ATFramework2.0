using ATFramework2._0.Driver;
using OpenQA.Selenium;

namespace Demo.Pages;

public interface IHomePage
{
    void click_SignInPortal();
}

class HomePage : IHomePage
{
    private readonly IDriverWait _driver;

    public HomePage(IDriverWait driver)
    {
        _driver = driver;
    }
    
    public IWebElement menu_input => _driver.FindElement(By.CssSelector("#menuToggle > input[type=checkbox]"));
    public IWebElement lnkSignInPortal => _driver.FindElement(By.LinkText("Sign In Portal"));
    
    public void click_SignInPortal()
    {
        menu_input.Click();
        lnkSignInPortal.Click();
    }
}





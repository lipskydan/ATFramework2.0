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
        _webDriver.OpenApplicationStartPage();
    }
    
    public IWebElement menuBtn => _webDriver.FindElement(By.CssSelector("#menuToggle > input[type=checkbox]"));
    public IWebElement lnkSignInPortal => _webDriver.FindElement(By.LinkText("Sign In Portal"));
    
    public void click_SignInPortal()
    {
        menuBtn.Click();
        lnkSignInPortal.Click();
    }
}





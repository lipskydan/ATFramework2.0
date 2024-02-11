using ATFramework2._0.ElementHandle;

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
    
    public Element menuBtn => _webDriver.ElementFinder.Css("#menuToggle > input[type=checkbox]");
    public Element lnkSignInPortal => _webDriver.ElementFinder.LinkText("Sign In Portal");
    
    public void click_SignInPortal()
    {
        menuBtn.Click();
        lnkSignInPortal.Click();
    }
}





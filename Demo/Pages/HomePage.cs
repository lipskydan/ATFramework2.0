using ATFramework2._0.ElementHandle;

namespace DemoUI.Pages;

public interface IHomePage
{
    void OpenSignInPortalPage();
}

class HomePage : IHomePage
{
    private readonly IWebDriverManager _webDriver;

    public HomePage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
        _webDriver.OpenApplicationStartPage();
    }
    
    public Element MenuBtn => _webDriver.ElementFinder.Css("#menuToggle > input[type=checkbox]");
    public Element LnkSignInPortal => _webDriver.ElementFinder.Css("[href*='SignIn']");
    
    public void OpenSignInPortalPage()
    {
        MenuBtn.Click();
        LnkSignInPortal.Click();
    }
}





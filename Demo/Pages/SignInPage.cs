using ATFramework2._0.ElementHandle;

namespace Demo.Pages;

public interface ISignInPage
{
    void clickNewRegistration();
    // void clickLogin();
    // void enterUserName();
    // void enterPassword();
}

public class SignInPage : ISignInPage
{
    private readonly IWebDriverManager _webDriver;

    public SignInPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }
    
    public int txtpwdlength => _webDriver.ElementsFinder.Id("pwd").Count();
    public int btnLogin => _webDriver.ElementsFinder.XPath("//input[@value='Login']").Count();
    public int btnRegistration => _webDriver.ElementsFinder.Id("NewRegistration").Count();

    public Element btnNewRegistration => _webDriver.ElementFinder.Id("NewRegistration");
    public Element btnLgn => _webDriver.ElementFinder.XPath("//*[@id=\"second_form\"]/input");
    public Element txtUserName => _webDriver.ElementFinder.XPath("//*[@id=\"usr\"]");
    public Element txtPassword => _webDriver.ElementFinder.XPath("//*[@id=\"pwd\"]");
    public string txtUsrPwdErrorMsg => _webDriver.ElementFinder.XPath("//*[@id=\"second_form\"]/div[2]/span").Text;

    public void clickNewRegistration()
    {
        btnNewRegistration.Click();
    }

    // public void clickLogin()
    // {
    //     btnLgn.Click();
    // }
    //
    // public void enterUserName()
    // {
    //     txtUserName.SendKeys(ConfigurationManager.AppSettings["Username"]);
    // }
    // public void enterPassword()
    // {
    //     txtPassword.SendKeys(ConfigurationManager.AppSettings["Password"]);
    // }
}
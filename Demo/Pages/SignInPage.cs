using ATFramework2._0.ElementHandle;

namespace DemoUI.Pages;

public interface ISignInPage
{
    void ClickNewRegistrationBtn();
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
    
    public int Txtpwdlength => _webDriver.ElementsFinder.Id("pwd").Count();
    public int BtnLogin => _webDriver.ElementsFinder.XPath("//input[@value='Login']").Count();
    public int BtnRegistration => _webDriver.ElementsFinder.Id("NewRegistration").Count();

    public Element BtnNewRegistration => _webDriver.ElementFinder.Id("NewRegistration");
    public Element BtnLgn => _webDriver.ElementFinder.XPath("//*[@id=\"second_form\"]/input");
    public Element TxtUserName => _webDriver.ElementFinder.XPath("//*[@id=\"usr\"]");
    public Element TxtPassword => _webDriver.ElementFinder.XPath("//*[@id=\"pwd\"]");
    public string TxtUsrPwdErrorMsg => _webDriver.ElementFinder.XPath("//*[@id=\"second_form\"]/div[2]/span").Text;

    public void ClickNewRegistrationBtn()
    {
        BtnNewRegistration.Click();
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
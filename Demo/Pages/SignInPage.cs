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
    
    public int txtuserlength => _webDriver.FindElements(By.Id("usr")).Count();
    public int txtpwdlength => _webDriver.FindElements(By.Id("pwd")).Count();
    public int btnLogin => _webDriver.FindElements(By.XPath("//input[@value='Login']")).Count();
    public int btnRegistration => _webDriver.FindElements(By.Id("NewRegistration")).Count();

    public IWebElement btnNewRegistration => _webDriver.FindElement(By.Id("NewRegistration"));
    public IWebElement btnLgn => _webDriver.FindElement(By.XPath("//*[@id=\"second_form\"]/input"));
    public IWebElement txtUserName => _webDriver.FindElement(By.XPath("//*[@id=\"usr\"]"));
    public IWebElement txtPassword => _webDriver.FindElement(By.XPath("//*[@id=\"pwd\"]"));
    public string txtUsrPwdErrorMsg => _webDriver.FindElement(By.XPath("//*[@id=\"second_form\"]/div[2]/span")).Text;

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
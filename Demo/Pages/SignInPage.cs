using ATFramework2._0.Driver;
using OpenQA.Selenium;
using System.Configuration;    

namespace Demo.Pages;

public interface ISignInPage
{
    void clickNewRegistration();
    void clickLogin();
    void enterUserName();
    void enterPassword();
}

public class SignInPage : ISignInPage
{
    private readonly IDriverWait _driver;

    public SignInPage(IDriverWait driver)
    {
        _driver = driver;
    }
    
    public int txtuserlength => _driver.FindElements(By.Id("usr")).Count();
    public int txtpwdlength => _driver.FindElements(By.Id("pwd")).Count();
    public int btnLogin => _driver.FindElements(By.XPath("//input[@value='Login']")).Count();
    public int btnRegistration => _driver.FindElements(By.Id("NewRegistration")).Count();

    public IWebElement btnNewRegistration => _driver.FindElement(By.Id("NewRegistration"));
    public IWebElement btnLgn => _driver.FindElement(By.XPath("//*[@id=\"second_form\"]/input"));
    public IWebElement txtUserName => _driver.FindElement(By.XPath("//*[@id=\"usr\"]"));
    public IWebElement txtPassword => _driver.FindElement(By.XPath("//*[@id=\"pwd\"]"));
    public string txtUsrPwdErrorMsg => _driver.FindElement(By.XPath("//*[@id=\"second_form\"]/div[2]/span")).Text;

    public void clickNewRegistration()
    {
        btnNewRegistration.Click();
    }

    public void clickLogin()
    {
        btnLgn.Click();
    }

    public void enterUserName()
    {
        txtUserName.SendKeys(ConfigurationManager.AppSettings["Username"]);
    }
    public void enterPassword()
    {
        txtPassword.SendKeys(ConfigurationManager.AppSettings["Password"]);
    }
}
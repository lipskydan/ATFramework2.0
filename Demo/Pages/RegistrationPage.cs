using ATFramework2._0.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace Demo.Pages;

public interface IRegistrationPage
{
    void select_Salutation();
    void click_Submit();
    void enter_FirstName();
    void enter_LastName();
    void enter_InvalidEmail();
    void enter_ValidEmail();
    void enter_UsrName();
    void enter_Password();
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IDriverWait _driver;

    public RegistrationPage(IDriverWait driver)
    {
        _driver = driver;
    } 
    
    public  IWebElement btnSubmit => _driver.FindElement(By.XPath("//input[@value='Submit']")); 
    public  IWebElement txtFirstName => _driver.FindElement(By.Id("firstname"));
    public  IWebElement txtLastName => _driver.FindElement(By.Id("lastname")); 
    public  IWebElement txtEmailid => _driver.FindElement(By.Id("emailId"));
    public  IWebElement txtUsername => _driver.FindElement(By.Id("usr"));
    public  IWebElement txtPassword => _driver.FindElement(By.Id("pwd"));
    public  string txtErrorMsg => _driver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;
    public  string txtErrorMsg2 => _driver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;

    public void select_Salutation()
    { 
        SelectElement drpSalutation = new SelectElement(_driver.FindElement(By.Id("Salutation")));
        drpSalutation.SelectByText(ConfigurationManager.AppSettings["Salutation"]);
    }
    
    public void click_Submit()
    {
        btnSubmit.Click();
    }
    public void enter_FirstName()
    {
        txtFirstName.SendKeys(ConfigurationManager.AppSettings["FirstName"]);
    }
    public void enter_LastName()
    {
        txtLastName.SendKeys(ConfigurationManager.AppSettings["LastName"]);
    }
    public void enter_InvalidEmail()
    {
        txtEmailid.SendKeys(ConfigurationManager.AppSettings["InvalidEmailAddress"]);
    }
    public void enter_ValidEmail()
    {
        txtEmailid.SendKeys(ConfigurationManager.AppSettings["ValidEmailAddress"]);
    }
    public void enter_UsrName()
    {
        txtUsername.SendKeys(ConfigurationManager.AppSettings["Username"]);
    }
    public void enter_Password()
    {
        txtPassword.SendKeys(ConfigurationManager.AppSettings["Password"]);
    }
}
using ATFramework2._0.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace Demo.Pages;

public interface IRegistrationPage
{
    void select_Salutation(string text);
    void click_Submit();
    void enter_FirstName(string text);
    void enter_LastName(string text);
    void enter_InvalidEmail(string text);
    void enter_ValidEmail(string text);
    void enter_UsrName(string text);
    void enter_Password(string text);
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public RegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    } 
    
    public  IWebElement btnSubmit => _webDriver.FindElement(By.XPath("//input[@value='Submit']")); 
    public  IWebElement txtFirstName => _webDriver.FindElement(By.Id("firstname"));
    public  IWebElement txtLastName => _webDriver.FindElement(By.Id("lastname")); 
    public  IWebElement txtEmailid => _webDriver.FindElement(By.Id("emailId"));
    public  IWebElement txtUsername => _webDriver.FindElement(By.Id("usr"));
    public  IWebElement txtPassword => _webDriver.FindElement(By.Id("pwd"));
    public  string txtErrorMsg => _webDriver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;
    public  string txtErrorMsg2 => _webDriver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;

    public void select_Salutation(string text)
    { 
        SelectElement drpSalutation = new SelectElement(_webDriver.FindElement(By.Id("Salutation")));
        drpSalutation.SelectByText(text);
    }
    
    public void click_Submit()
    {
        btnSubmit.Click();
    }
    public void enter_FirstName(string text)
    {
        txtFirstName.SendKeys(text);
    }
    public void enter_LastName(string text)
    {
        txtLastName.SendKeys(text);
    }
    public void enter_InvalidEmail(string text)
    {
        txtEmailid.SendKeys(text);
    }
    public void enter_ValidEmail(string text)
    {
        txtEmailid.SendKeys(text);
    }
    public void enter_UsrName(string text)
    {
        txtUsername.SendKeys(text);
    }
    public void enter_Password(string text)
    {
        txtPassword.SendKeys(text);
    }
}
using ATFramework2._0.ElementHandle;

namespace Demo.Pages;

public interface IRegistrationPage
{
    void SelectSalutation(string text);
    void ClickSubmitBtn();
    void InputFirstName(string text);
    void InputLastName(string text);
    void InputEmail(string text);
    void InputUserName(string text);
    void InputPassword(string text);
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public RegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }

    public Element btnSubmit => _webDriver.ElementFinder.XPath("//input[@value='Submit']"); 
    public Element txtFirstName => _webDriver.ElementFinder.Id("firstname");
    public  Element txtLastName => _webDriver.ElementFinder.Id("lastname"); 
    public  Element txtEmailid => _webDriver.ElementFinder.Id("emailId");
    public  Element txtUsername => _webDriver.ElementFinder.Id("usr");
    public  Element txtPassword => _webDriver.ElementFinder.Id("pwd");
    public  string txtErrorMsg => _webDriver.ElementFinder.XPath("//*[@id=\"first_form\"]/div/span").Text;
    public  string txtErrorMsg2 => _webDriver.ElementFinder.XPath("//*[@id=\"first_form\"]/div/span").Text;

    public void SelectSalutation(string text)
    { 
        SelectElement drpSalutation = new SelectElement(_webDriver.ElementFinder._Id("Salutation"));
        drpSalutation.SelectByText(text);
    }
    
    public void ClickSubmitBtn()
    {
        btnSubmit.Click();
    }
    public void InputFirstName(string text)
    {
        txtFirstName.SendKeys(text);
    }
    public void InputLastName(string text)
    {
        txtLastName.SendKeys(text);
    }
    public void InputEmail(string text)
    {
        txtEmailid.SendKeys(text);
    }
    public void InputUserName(string text)
    {
        txtUsername.SendKeys(text);
    }
    public void InputPassword(string text)
    {
        txtPassword.SendKeys(text);
    }
}
using ATFramework2._0.ElementHandle;

namespace DemoUI.Pages;

public interface IRegistrationPage
{
    void SelectSalutation(string text);
    void ClickSubmitBtn();
    void InputFirstName(string text);
    void InputLastName(string text);
    void InputEmail(string text);
    void InputUserName(string text);
    void InputPassword(string text);
    string GetTxtErrorMsgField(string field);
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public RegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }

    public Element BtnSubmit => _webDriver.ElementFinder.XPath("//input[@value='Submit']"); 
    public Element TxtFirstName => _webDriver.ElementFinder.Id("firstname");
    public  Element TxtLastName => _webDriver.ElementFinder.Id("lastname"); 
    public  Element TxtEmailid => _webDriver.ElementFinder.Id("emailId");
    public  Element TxtUsername => _webDriver.ElementFinder.Id("usr");
    public  Element TxtPassword => _webDriver.ElementFinder.Id("pwd");
    public  string TxtErrorMsg => _webDriver.ElementFinder.XPath("//*[@id=\"first_form\"]/div/span").Text;
    public  string TxtErrorMsg2 => _webDriver.ElementFinder.XPath("//*[@id=\"first_form\"]/div/span").Text;

    #region ErrorMsgField
    private readonly Dictionary<string, string> _fieldIds = new Dictionary<string, string>
    {
        { "FirstName", "firstname" },
        { "LastName", "lastname" },
    };
    public string TxtErrorMsgField(string field) => _webDriver.ElementFinder.XPath($"//*[@id='{_fieldIds[field]}']/following-sibling::span[@class='error']").Text;
   public string GetTxtErrorMsgField(string field) => TxtErrorMsgField(field);
    #endregion
    
    public void SelectSalutation(string text)
    { 
        SelectElement drpSalutation = new SelectElement(_webDriver.ElementFinder._Id("Salutation"));
        drpSalutation.SelectByText(text);
    }
    
    public void ClickSubmitBtn()
    {
        BtnSubmit.Click();
    }
    public void InputFirstName(string text)
    {
        TxtFirstName.SendKeys(text);
    }
    public void InputLastName(string text)
    {
        TxtLastName.SendKeys(text);
    }
    public void InputEmail(string text)
    {
        TxtEmailid.SendKeys(text);
    }
    public void InputUserName(string text)
    {
        TxtUsername.SendKeys(text);
    }
    public void InputPassword(string text)
    {
        TxtPassword.SendKeys(text);
    }
}
using ATFramework2._0.ElementHandle;

namespace DemoUI.Pages;

public interface IRegistrationPage
{
    void SelectSalutation(string text);
    void ClickSubmitBtn();
    void InputTxtField(string fieldName, string text);
    string GetTxtErrorMsgField(string field);
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public RegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }

    private readonly Dictionary<string, string> _fieldIds = new Dictionary<string, string>
    {
        { "FirstName", "firstname" },
        { "LastName",  "lastname"  },
        { "Email",     "emailId"   },
        { "UserName",  "usr"       },
        { "Password",  "pwd"       },
    };

    #region TxtField
    public Element TxtField(string fieldName) => _webDriver.ElementFinder.Id(_fieldIds[fieldName]);
    void IRegistrationPage.InputTxtField(string fieldName, string text)
    {
        TxtField(fieldName).SendKeys(text);
    }
    #endregion

    #region ErrorMsgField
    public string txtErrorMsgField(string fieldName) => _webDriver.ElementFinder.XPath($"//*[@id='{_fieldIds[fieldName]}']/following-sibling::span[@class='error']").Text;
    public string GetTxtErrorMsgField(string fieldName) => txtErrorMsgField(fieldName);
    #endregion

    #region DropDownSalutation
    public Element dropDownSalutationButton => _webDriver.ElementFinder.Css("[id='Salutation']");
    public Element optionSalutation(string optionName) => _webDriver.ElementFinder.Css($"option[value='{optionName}']");
     public void SelectSalutation(string text)
    { 
        dropDownSalutationButton.Click();
        optionSalutation(text).Click();
    }
    #endregion

    public Element btnSubmit => _webDriver.ElementFinder.XPath("//input[@value='Submit']"); 
    public void ClickSubmitBtn()
    {
        btnSubmit.Click();
    }
}
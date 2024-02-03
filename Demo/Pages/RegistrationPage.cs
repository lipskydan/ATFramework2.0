namespace Demo.Pages;

public interface IRegistrationPage
{
    void SelectSalutation(string text);
    void clickSubmitBtn();
    void inputFirstName(string text);
    void inputLastName(string text);
    void inputEmail(string text);
    void inputUserName(string text);
    void inputPassword(string text);
}

public class RegistrationPage: IRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public RegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    }

    public IWebElement btnSubmit => _webDriver.ElementFinder.XPath("//input[@value='Submit']"); // _webDriver.FindElement(By.XPath("//input[@value='Submit']")); 
    public  IWebElement txtFirstName => _webDriver.FindElement(By.Id("firstname")); //[id='firstname']
    public  IWebElement txtLastName => _webDriver.FindElement(By.Id("lastname")); 
    public  IWebElement txtEmailid => _webDriver.FindElement(By.Id("emailId"));
    public  IWebElement txtUsername => _webDriver.FindElement(By.Id("usr"));
    public  IWebElement txtPassword => _webDriver.FindElement(By.Id("pwd"));
    public  string txtErrorMsg => _webDriver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;
    public  string txtErrorMsg2 => _webDriver.FindElement(By.XPath("//*[@id=\"first_form\"]/div/span")).Text;

    public void SelectSalutation(string text)
    { 
        SelectElement drpSalutation = new SelectElement(_webDriver.FindElement(By.Id("Salutation")));
        drpSalutation.SelectByText(text);
    }
    
    public void clickSubmitBtn()
    {
        btnSubmit.Click();
    }
    public void inputFirstName(string text)
    {
        txtFirstName.SendKeys(text);
    }
    public void inputLastName(string text)
    {
        txtLastName.SendKeys(text);
    }
    public void inputEmail(string text)
    {
        txtEmailid.SendKeys(text);
    }
    public void inputUserName(string text)
    {
        txtUsername.SendKeys(text);
    }
    public void inputPassword(string text)
    {
        txtPassword.SendKeys(text);
    }
}
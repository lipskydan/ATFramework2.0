namespace DemoUI.Steps;

[Binding]
public class UserRegistrationSteps
{
    private readonly IHomePage _homePage;
    private readonly ISignInPage _signInPage;
    private readonly IRegistrationPage _registrationPage;
    private readonly ISuccessRegistrationPage _successRegistrationPage;

    private readonly IWebDriverManager _webDriver;

    public UserRegistrationSteps(IWebDriverManager webDriver, IHomePage homePage, ISignInPage signInPage, IRegistrationPage registrationPage, ISuccessRegistrationPage successRegistrationPage)
    {
        _webDriver = webDriver;

        _homePage = homePage;
        _signInPage = signInPage;
        _registrationPage = registrationPage;
        _successRegistrationPage = successRegistrationPage;
    }
    
    [Given(@"Navigate to the start page of the app")]
    public void GivenNavigateToTheApp()
    {
        
    }

    [Given(@"Open ""(.*)"" page")]
    public void OpenPage(string pageName)
    {
        switch (pageName)
        {
            case "Sign In":
                _homePage.OpenSignInPortalPage();
                break;
        }
    }

    [Given(@"Click on the button ""(.*)""")]
    public void GivenClickOnTheButton(string btnName)
    {
        switch (btnName)
        {
            case "New Registration":
                _signInPage.ClickNewRegistrationBtn();
                break;
        }
    }

    [Given(@"Select Salutation ""(.*)""")]
    public void GivenSelectSalutation(string salutationName)
    {
        _registrationPage.SelectSalutation(salutationName);
    }

    [Given(@"Enter text ""(.*)"" to the field ""(.*)""")]
    public void EnterTextToField(string text, string fieldName)
    {
        _registrationPage.InputTxtField(fieldName, text);
    }
    
    [When(@"Click on the button ""(.*)""")]
    public void WhenClickOnTheButton(string submit)
    {
        switch (submit)
        {
            case "Submit":
                _registrationPage.ClickSubmitBtn();
                break;
        }
    }

    [Then(@"Success message ""(.*)"" is displayed")]
    public void ThenSuccessMessageIsDisplayed(string expText)
    {
        VerifyWorker.Equals(exp: expText, act: () => _successRegistrationPage.GetSuccessMsg(), 
        message: $"Current message should be '{expText}'");
    }

    [Then(@"Fail message ""(.*)"" under the field ""(.*)"" is displayed")]
    public void ThenFailMessageIsDisplayed(string expText, string fieldName)
    {
        VerifyWorker.Equals(exp: expText, act: () => _registrationPage.GetTxtErrorMsgField(fieldName));
    }
}
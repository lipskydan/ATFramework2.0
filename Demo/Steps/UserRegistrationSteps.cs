namespace Demo.Steps;

[Binding]
public class UserRegistrationSteps
{
    private readonly ScenarioContext _scenarioContext;
    private readonly IHomePage _homePage;
    private readonly ISignInPage _signInPage;
    private readonly IRegistrationPage _registrationPage;
    private readonly ISuccessRegistrationPage _successRegistrationPage;

    public UserRegistrationSteps(ScenarioContext scenarioContext, IHomePage homePage, ISignInPage signInPage, IRegistrationPage registrationPage, ISuccessRegistrationPage successRegistrationPage)
    {
        _scenarioContext = scenarioContext;
        _homePage = homePage;
        _signInPage = signInPage;
        _registrationPage = registrationPage;
        _successRegistrationPage = successRegistrationPage;
    }
    
    [Given(@"Navigate to the start page of the app")]
    public void GivenNavigateToTheApp()
    {
        Console.WriteLine("Navigate to the app");
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

    [Given(@"Enter FirstName ""(.*)""")]
    public void GivenEnterFirstName(string firstName)
    {
        _registrationPage.InputFirstName(firstName);
    }

    [Given(@"Enter LastName ""(.*)""")]
    public void GivenEnterLastName(string lastName)
    {
        _registrationPage.InputLastName(lastName);
    }

    [Given(@"Enter Email ""(.*)""")]
    public void GivenEnterEmail(string email)
    {
        _registrationPage.InputEmail(email);
    }

    [Given(@"Enter UserName ""(.*)""")]
    public void GivenEnterUserName(string userName)
    {
        _registrationPage.InputUserName(userName);
    }

    [Given(@"Enter Password ""(.*)""")]
    public void GivenEnterPassword(string password)
    {
        _registrationPage.InputPassword(password);
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
    public void ThenSuccessMessageIsDisplayed(string actText)
    {
        Verify.StringEquals(exp: _successRegistrationPage.getSuccessMsg(), act: actText);
    }
}
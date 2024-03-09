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

    [Given(@"Select Salutation")]
    public void GivenSelectSalutation()
    {
        _registrationPage.SelectSalutation("Mr.");
    }

    [Given(@"Enter FirstName")]
    public void GivenEnterFirstName()
    {
        _registrationPage.InputFirstName("Clark");
    }

    [Given(@"Enter LastName")]
    public void GivenEnterLastName()
    {
        _registrationPage.InputLastName("Smith");
    }

    [Given(@"Enter ValidEmail")]
    public void GivenEnterValidEmail()
    {
        _registrationPage.InputEmail("PeterSmith@gmail.com");
    }

    [Given(@"Enter UserName")]
    public void GivenEnterUserName()
    {
        _registrationPage.InputUserName("Peter");
    }

    [Given(@"Enter Password")]
    public void GivenEnterPassword()
    {
        _registrationPage.InputPassword("Password");
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
    public void ThenSuccessMessageIsDisplayed(string p0)
    {
        Assert.AreEqual("User Registered Successfully !!!", _successRegistrationPage.getSuccessMsg());
    }
}
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

    [Given(@"click on the button ""(.*)""")]
    public void GivenClickOnTheButton(string btnName)
    {
        switch (btnName)
        {
            case "Sign In Portal":
                _homePage.click_SignInPortal();
                break;
            case "New Registration":
                _signInPage.clickNewRegistration();
                break;
        }
    }

    [Given(@"select Salutation")]
    public void GivenSelectSalutation()
    {
        _registrationPage.SelectSalutation("Mr.");
    }

    [Given(@"enter FirstName")]
    public void GivenEnterFirstName()
    {
        _registrationPage.inputFirstName("Clark");
    }

    [Given(@"enter LastName")]
    public void GivenEnterLastName()
    {
        _registrationPage.inputLastName("Smith");
    }

    [Given(@"enter ValidEmail")]
    public void GivenEnterValidEmail()
    {
        _registrationPage.inputEmail("PeterSmith@gmail.com");
    }

    [Given(@"enter UserName")]
    public void GivenEnterUserName()
    {
        _registrationPage.inputUserName("Peter");
    }

    [Given(@"enter Password")]
    public void GivenEnterPassword()
    {
        _registrationPage.inputPassword("Password");
    }

    [When(@"click on the button ""(.*)""")]
    public void WhenClickOnTheButton(string submit)
    {
        switch (submit)
        {
            case "Submit":
                _registrationPage.clickSubmitBtn();
                break;
        }
    }

    [Then(@"Success message ""(.*)"" is displayed")]
    public void ThenSuccessMessageIsDisplayed(string p0)
    {
        Assert.AreEqual("User Registered Successfully !!!", _successRegistrationPage.getSuccessMsg());
    }
}
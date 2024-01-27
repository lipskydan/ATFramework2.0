using Demo.Pages;
using NUnit.Framework;
using TechTalk.SpecFlow;

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
    
    [Given(@"Navigate to the app")]
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
        _registrationPage.select_Salutation("Mr.");
    }

    [Given(@"enter FirstName")]
    public void GivenEnterFirstName()
    {
        _registrationPage.enter_FirstName("Clark");
    }

    [Given(@"enter LastName")]
    public void GivenEnterLastName()
    {
        _registrationPage.enter_LastName("Smith");
    }

    [Given(@"enter ValidEmail")]
    public void GivenEnterValidEmail()
    {
        _registrationPage.enter_ValidEmail("PeterSmith@gmail.com");
    }

    [Given(@"enter UserName")]
    public void GivenEnterUserName()
    {
        _registrationPage.enter_UsrName("Peter");
    }

    [Given(@"enter Password")]
    public void GivenEnterPassword()
    {
        _registrationPage.enter_Password("Password");
    }

    [When(@"click on the button ""(.*)""")]
    public void WhenClickOnTheButton(string submit)
    {
        switch (submit)
        {
            case "Submit":
                _registrationPage.click_Submit();
                break;
        }
    }

    [Then(@"Success message ""(.*)"" is displayed")]
    public void ThenSuccessMessageIsDisplayed(string p0)
    {
        Assert.AreEqual("User Registered Successfully !!!", _successRegistrationPage.get_SuccessMsg());
    }
}
namespace DemoUI.Steps;

[Binding]
public class UserRegistrationSteps
{
    private readonly IHomePage _homePage;
    private readonly ISignInPage _signInPage;
    private readonly IRegistrationPage _registrationPage;
    private readonly ISuccessRegistrationPage _successRegistrationPage;
    private readonly IWebDriverManager _webDriver;

    public UserRegistrationSteps(
        IWebDriverManager webDriver,
        IHomePage homePage,
        ISignInPage signInPage,
        IRegistrationPage registrationPage,
        ISuccessRegistrationPage successRegistrationPage)
    {
        _webDriver = webDriver;
        _homePage = homePage;
        _signInPage = signInPage;
        _registrationPage = registrationPage;
        _successRegistrationPage = successRegistrationPage;
    }

    private void Log(string message, LogLevel level = LogLevel.Info, string feature = "User Registration")
    {
        _webDriver.LogWorker.Log(message, level, feature, ScenarioContext.Current.ScenarioInfo.Title);
    }

    private void LogException(Exception ex, string action)
    {
        Log($"Exception during '{action}': {ex.Message}\n{ex.StackTrace}", LogLevel.Error);
    }

    [Given(@"Navigate to the start page of the app")]
    public void GivenNavigateToTheApp()
    {
        try
        {
            Log("Navigating to the start page of the app.");
        }
        catch (Exception ex)
        {
            LogException(ex, "Navigating to the start page");
            throw;
        }
    }

    [Given(@"Open ""(.*)"" page")]
    public void OpenPage(string pageName)
    {
        try
        {
            Log($"Opening the '{pageName}' page.");

            switch (pageName)
            {
                case "Sign In":
                    _homePage.OpenSignInPortalPage();
                    break;
                default:
                    Log($"Page '{pageName}' not found.", LogLevel.Warning);
                    break;
            }
        }
        catch (Exception ex)
        {
            LogException(ex, $"Opening page '{pageName}'");
            throw;
        }
    }

    [Given(@"Click on the button ""(.*)""")]
    public void GivenClickOnTheButton(string btnName)
    {
        try
        {
            Log($"Clicking on the '{btnName}' button.");

            switch (btnName)
            {
                case "New Registration":
                    _signInPage.ClickNewRegistrationBtn();
                    break;
                default:
                    Log($"Button '{btnName}' not recognized.", LogLevel.Warning);
                    break;
            }
        }
        catch (Exception ex)
        {
            LogException(ex, $"Clicking button '{btnName}'");
            throw;
        }
    }

    [Given(@"Select Salutation ""(.*)""")]
    public void GivenSelectSalutation(string salutationName)
    {
        try
        {
            Log($"Selecting salutation: {salutationName}.");
            _registrationPage.SelectSalutation(salutationName);
        }
        catch (Exception ex)
        {
            LogException(ex, $"Selecting salutation '{salutationName}'");
            throw;
        }
    }

    [Given(@"Enter text ""(.*)"" to the field ""(.*)""")]
    public void EnterTextToField(string text, string fieldName)
    {
        try
        {
            Log($"Entering '{text}' into the '{fieldName}' field.");
            _registrationPage.InputTxtField(fieldName, text);
        }
        catch (Exception ex)
        {
            LogException(ex, $"Entering text into field '{fieldName}'");
            throw;
        }
    }

    [When(@"Click on the button ""(.*)""")]
    public void WhenClickOnTheButton(string submit)
    {
        try
        {
            Log($"Clicking on the '{submit}' button.");

            switch (submit)
            {
                case "Submit":
                    _registrationPage.ClickSubmitBtn();
                    break;
                default:
                    Log($"Button '{submit}' not recognized.", LogLevel.Warning);
                    break;
            }
        }
        catch (Exception ex)
        {
            LogException(ex, $"Clicking button '{submit}'");
            throw;
        }
    }

    [Then(@"Success message ""(.*)"" is displayed")]
    public void ThenSuccessMessageIsDisplayed(string expText)
    {
        VerifyWorker.StringsEqual(
            logWorker:  _webDriver.LogWorker,
            expected: expText,
            actual: _successRegistrationPage.GetSuccessMsg(),
            message: $"Current message should be '{expText}'"
            );
    }

    [Then(@"Fail message ""(.*)"" under the field ""(.*)"" is displayed")]
    public void ThenFailMessageIsDisplayed(string expText, string fieldName)
    {
        VerifyWorker.StringsEqual(
            logWorker:  _webDriver.LogWorker,
            expected: expText,
            actual: _registrationPage.GetTxtErrorMsgField(fieldName)
            );
    }
}
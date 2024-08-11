namespace DemoUI.Pages;

public interface ISuccessRegistrationPage
{
    string GetSuccessMsg();
}

public class SuccessRegistrationPage : ISuccessRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public SuccessRegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    } 
    public string GetSuccessMsg() => _webDriver.ElementFinder.XPath("/html/body/center[1]/h1").Text;
}
namespace Demo.Pages;

public interface ISuccessRegistrationPage
{
    string getSuccessMsg();
}

public class SuccessRegistrationPage : ISuccessRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public SuccessRegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    } 
    public string getSuccessMsg() => _webDriver.ElementFinder.XPath("/html/body/center[1]/h1").Text;
}
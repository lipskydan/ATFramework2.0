using ATFramework2._0.Driver;
using OpenQA.Selenium;

namespace Demo.Pages;

public interface ISuccessRegistrationPage
{
    string get_SuccessMsg();
}

public class SuccessRegistrationPage : ISuccessRegistrationPage
{
    private readonly IWebDriverManager _webDriver;

    public SuccessRegistrationPage(IWebDriverManager webDriver)
    {
        _webDriver = webDriver;
    } 
    public string get_SuccessMsg() => _webDriver.FindElement(By.XPath("/html/body/center[1]/h1")).Text;
}
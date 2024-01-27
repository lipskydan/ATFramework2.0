using ATFramework2._0.Driver;
using OpenQA.Selenium;

namespace Demo.Pages;

public interface ISuccessRegistrationPage
{
    string get_SuccessMsg();
}

public class SuccessRegistrationPage : ISuccessRegistrationPage
{
    private readonly IDriverWait _driver;

    public SuccessRegistrationPage(IDriverWait driver)
    {
        _driver = driver;
    } 
    public string get_SuccessMsg() => _driver.FindElement(By.XPath("/html/body/center[1]/h1")).Text;
}
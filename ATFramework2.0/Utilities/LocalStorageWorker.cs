namespace ATFramework2._0;

public class LocalStorageWorker
{
    private IWebDriverManager _driverManger;
    private IJavaScriptExecutor js;

    public LocalStorageWorker(IWebDriverManager driverManger)
    {
        _driverManger = driverManger;
        js = (IJavaScriptExecutor)_driverManger.Driver;
    }

    public Dictionary<string, string> GetLocalStorage()
    {   
        return (Dictionary<string, string>)js.ExecuteScript("return window.localStorage;");
    }
    public void AddToLocalStorage(string key, string value)
    {
        js.ExecuteScript($"window.localStorage.setItem('{key}', '{value}');");
    }
    public void UpdateLocalStorageValue(string key, string newValue)
    {
        js.ExecuteScript($"window.localStorage.setItem('{key}', '{newValue}');");
    }
    public void DeleteFromLocalStorage(string key)
    {
        js.ExecuteScript($"window.localStorage.removeItem('{key}');");
    }
}
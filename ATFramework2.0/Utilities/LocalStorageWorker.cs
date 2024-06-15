namespace ATFramework2._0;

public class LocalStorageWorker
{
    private IJavaScriptExecutor js;

    public LocalStorageWorker(IWebDriver driver)
    {
        js = (IJavaScriptExecutor)driver;
    }

    public Dictionary<string, string> GetLocalStorage()
    {   
        var localStorage = (Dictionary<string, object>)js.ExecuteScript(@"
            let items = {}; 
            for (let i = 0; i < localStorage.length; i++) {
                let key = localStorage.key(i);
                items[key] = localStorage.getItem(key);
            }
            return items;
        ");

        // Convert Dictionary<string, object> to Dictionary<string, string>
        var result = new Dictionary<string, string>();
        foreach (var kvp in localStorage)
        {
            result.Add(kvp.Key, kvp.Value.ToString());
        }

        return result;
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
     public void ClearLocalStorage()
    {
        js.ExecuteScript("window.localStorage.clear();");
    }
}
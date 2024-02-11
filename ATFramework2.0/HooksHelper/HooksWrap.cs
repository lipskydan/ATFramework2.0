namespace ATFramework2._0.HooksHelper;

[Binding]
public class HooksWrap
{
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly IWebDriverManager _webDriverManager;
    
    public HooksWrap(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _webDriverManager = webDriverManager;
    }
    
    [BeforeScenario]
    public void BeforeScenario()
    {
        
    }

    [AfterScenario]
    public void AfterScenario()
    {
        
    }
}
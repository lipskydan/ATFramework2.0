using ATFramework2._0.Driver;
using TechTalk.SpecFlow;

namespace Demo.Hooks;

[Binding]
public class Initialization
{
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly IWebDriverManager _webDriverManager;
    
    public Initialization(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager)
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
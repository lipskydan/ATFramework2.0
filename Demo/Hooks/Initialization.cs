using ATFramework2._0.Driver;
using TechTalk.SpecFlow;

namespace Demo.Hooks;

[Binding]
public class Initialization
{
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private readonly IDriverFixture _driverFixture;
    
    public Initialization(ScenarioContext scenarioContext, FeatureContext featureContext, IDriverFixture driverFixture)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _driverFixture = driverFixture;
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
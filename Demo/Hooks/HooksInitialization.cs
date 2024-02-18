namespace Demo.Hooks;

[Binding]
public class HooksInitialization: HooksWrap
{
    public HooksInitialization(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager) 
        : base(scenarioContext, featureContext, webDriverManager)
    {
        
    }
}
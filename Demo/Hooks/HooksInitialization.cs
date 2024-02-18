namespace Demo.Hooks;

[Binding]
public class HooksInitialization: HooksWrap
{
    public HooksInitialization(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager, TestSettings testSettings) 
        : base(scenarioContext, featureContext, webDriverManager, testSettings)
    {
        
    }
}
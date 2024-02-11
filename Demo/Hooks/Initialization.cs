namespace Demo.Hooks;

[Binding]
public class Initialization: HooksWrap
{
    public Initialization(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager) 
        : base(scenarioContext, featureContext, webDriverManager)
    {
        
    }
}
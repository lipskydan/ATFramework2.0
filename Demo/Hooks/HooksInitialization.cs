using ATFramework2._0.HooksWrap;

namespace DemoUI.Hooks;

[Binding]
public class HooksInitialization: HooksWrapReportGenerator
{
    public HooksInitialization(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager, TestSettings testSettings) 
        : base(scenarioContext, featureContext, webDriverManager, testSettings)
    {
        
    }
}
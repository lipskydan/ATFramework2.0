namespace DemoUI.Hooks;

using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;


[Binding]
public class HooksInitialization
{
    private readonly HtmlReportGenerator _reportGenerator;
    private readonly TestSettings _testSettings;
    
    public HooksInitialization()
    {
        var services = DiSetupBase.CreateBaseServices(out _);
        var serviceProvider = services.BuildServiceProvider();

        _testSettings = serviceProvider.GetService<TestSettings>();
        _reportGenerator = HtmlReportGenerator.Instance(_testSettings);
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext)
    {
        string scenarioName = scenarioContext.ScenarioInfo.Title;
        _reportGenerator.StartScenario(scenarioName);
    }

    [AfterStep]
    public void AfterStep(ScenarioContext scenarioContext)
    {
        string scenarioName = scenarioContext.ScenarioInfo.Title;
        string stepName = scenarioContext.StepContext.StepInfo.Text;

        if (scenarioContext.TestError != null)
        {
            _reportGenerator.AddStepResult(scenarioName, stepName, "Failed");
        }
        else
        {
            _reportGenerator.AddStepResult(scenarioName, stepName, "Passed");
        }

        Console.WriteLine($"[DL][Log] {scenarioName} - {stepName}");
    }

    [AfterScenario]
    public void AfterScenario(ScenarioContext scenarioContext)
    {
        string scenarioName = scenarioContext.ScenarioInfo.Title;
        _reportGenerator.EndScenario(scenarioName);
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        var testSettings = ConfigReader.ReadConfig();
        HtmlReportGenerator.Instance(testSettings).FinalizeReport();
    }
}

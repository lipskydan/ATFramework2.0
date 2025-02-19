using ATFramework2._0.Utilities.Logs;

namespace DemoUI.Hooks;

[Binding]
public class HooksInitialization
{
    private readonly FeatureContext _featureContext;

    private readonly HtmlReportGenerator _reportGenerator;
    private readonly TestSettings _testSettings;
    
    public HooksInitialization(FeatureContext featureContext)
    {
         _featureContext = featureContext;
        var services = DiSetupBase.CreateBaseServices(out _);
        var serviceProvider = services.BuildServiceProvider();
        _testSettings = serviceProvider.GetService<TestSettings>();
        _reportGenerator = HtmlReportGenerator.Instance(_testSettings); 
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        // Set the test run timestamp
        TestRunContext.Initialize();
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenarioContext)
    {
        string scenarioName = scenarioContext.ScenarioInfo.Title;
        string featureName = _featureContext.FeatureInfo.Title;
        _reportGenerator.StartScenario(featureName, scenarioName);
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

        // var services = DiSetupBase.CreateBaseServices(out _);
        // var serviceProvider = services.BuildServiceProvider();
        // var logWorker = serviceProvider.GetService<IWebDriverManager>().LogWorker;
        // var logs = logWorker.GetAllLogs();
        // //var logs = logWorker.GetLogsByLevels(new List<LogLevel> { LogLevel.Warning, LogLevel.Error, LogLevel.Critical });
        // var analyzer = new DynamicLogAnalyzer(logs);
        // var analysisResults = analyzer.AnalyzeLogs();

        var reportGenerator = HtmlReportGenerator.Instance(testSettings);
        // reportGenerator.AddLogAnalysisResults(
        //     logs.Select(log => log.Message).ToList(), 
        //     analysisResults);

        reportGenerator.FinalizeReport();
    }
}

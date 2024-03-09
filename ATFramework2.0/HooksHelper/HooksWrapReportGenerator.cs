namespace ATFramework2._0.HooksHelper;

[Binding]
public class HooksWrapReportGenerator
{
    protected readonly IWebDriverManager _webDriverManager;
    
    private ExtentTest? _scenario;
    private readonly ScenarioContext _scenarioContext;
    private readonly FeatureContext _featureContext;
    private static ExtentReports _extentReports;
    private static string _pathToSaveReport;
     private readonly TestSettings _testSettings;
    
    public HooksWrapReportGenerator(ScenarioContext scenarioContext, FeatureContext featureContext, IWebDriverManager webDriverManager, TestSettings testSettings)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
        _webDriverManager = webDriverManager;
        _testSettings = testSettings;
        _pathToSaveReport = _testSettings.PathToSaveReport;
    }
    
    [BeforeTestRun]
    public static void InitializeExtentReports()
    {
        var reportPath = Path.Combine(_pathToSaveReport, $"AT_report_1.html");
        _extentReports = new ExtentReports();
        var spark = new ExtentSparkReporter(reportPath);
        _extentReports.AttachReporter(spark);
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        if(_testSettings.ReportGenerate == true)
        {
            if (_extentReports == null)
            {
                InitializeExtentReports();
            }
            var feature = _extentReports?.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
            _scenario = feature?.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }
    }
    
    [AfterStep]
    public void AfterStep()
    {
        if(_testSettings.ReportGenerate == true)
        {
            var fileName =
            $"{_featureContext.FeatureInfo.Title.Trim()}_{Regex.Replace(_scenarioContext.ScenarioInfo.Title, @"\s", "")}";
            
            if(_scenarioContext.TestError == null)
                switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case StepDefinitionType.Given:
                        _scenario?.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case StepDefinitionType.When:
                        _scenario?.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    case StepDefinitionType.Then:
                        _scenario?.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
            {
                switch (_scenarioContext.StepContext.StepInfo.StepDefinitionType)
                {
                    case StepDefinitionType.Given:
                        _scenario?.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                            // .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                            // {
                            //     //Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            //     Title = "Error screenshot"
                            // });
                        
                        break;
                    case StepDefinitionType.When:
                        _scenario?.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                            // .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                            // {
                            //     //Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            //     Title = "Error screenshot"
                            // });
                        break;
                    case StepDefinitionType.Then:
                        _scenario?.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                            // .Fail(_scenarioContext.TestError.Message, new ScreenCapture()
                            // {
                            //     //Path = _driverFixture.TakeScreenshotAsPath(fileName),
                            //     Title = "Error screenshot"
                            // });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
    
    [AfterTestRun]
    public static void TearDownReport() => _extentReports?.Flush();
}
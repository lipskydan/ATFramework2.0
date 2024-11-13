namespace ATFramework2._0.Config;

public class TestSettings
{
    public BrowserType BrowserType { get; set; }
    public string? BrowserVersion { get; set; }
    public Uri? ApplicationUrl { get; set; }
    public float? TimeoutInterval { get; set; }
    
    public bool ReportGenerate { get; set; }
    public string? PathToSaveReport { get; set; }
    
    public TestRunType TestRunType { get; set; }
    public Uri? GridUri { get; set; }
}


public enum TestRunType
{
    Local,
    Grid
}
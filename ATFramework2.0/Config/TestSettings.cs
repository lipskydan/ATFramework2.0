namespace ATFramework2._0.Config
{
    public class TestSettings
    {
        public Uri? ApplicationUrl { get; set; }
        public BrowserSettings Browser { get; set; } = new();
        public ReportSettings Report { get; set; } = new();
        public LogsSettings Logs { get; set; } = new();
        public UtilitySettings Utilities { get; set; } = new();
    }

    public class BrowserSettings
    {
        public BrowserType Type { get; set; }
        public string? Version { get; set; }
    }

    public class ReportSettings
    {
        public bool ToGenerate { get; set; }
        public string? PathToSave { get; set; }
    }

    public class LogsSettings
    {
        public string? PathToSave { get; set; }
    }

    public class UtilitySettings
    {
        public float? TimeoutInterval { get; set; }
        public TestRunType TestRunType { get; set; }
        public Uri? GridUri { get; set; }
    }

    public enum TestRunType
    {
        Local,
        Grid
    }

    public enum BrowserType
    {
        Chrome,
        Firefox,
        Safari
    }
}

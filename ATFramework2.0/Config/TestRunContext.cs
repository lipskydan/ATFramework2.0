using System;

namespace ATFramework2._0.Config;

public static class TestRunContext
{
    public static string TestRunTimestamp { get; private set; }

    public static void Initialize()
    {
        TestRunTimestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
    }
}

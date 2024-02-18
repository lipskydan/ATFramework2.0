namespace ATFramework2._0;

public class DiSetupBase
{
    protected static IServiceCollection CreateBaseServices(out IServiceCollection services)
    {
        services = new ServiceCollection();
        
        services
            .AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IWebDriverManager, Driver.WebDriverManager>();
        
        return services;
    }
}



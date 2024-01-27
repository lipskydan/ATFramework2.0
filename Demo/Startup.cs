using Demo.Pages;
//using ATFramework2._0.Config;
using ATFramework2._0.Driver;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;

namespace Demo;

public class Startup
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        services
            // .AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IDriverFixture, DriverFixture>()
            .AddScoped<IDriverWait, DriverWait>()
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IRegistrationPage, RegistrationPage>()
            .AddScoped<ISignInPage, SignInPage>()
            .AddScoped<ISuccessRegistrationPage, SuccessRegistrationPage>();

        return services;
    }
    
}
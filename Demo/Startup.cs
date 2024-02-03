namespace Demo;

public class Startup
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        services
            .AddSingleton(ConfigReader.ReadConfig())
            .AddScoped<IWebDriverManager, WebWebDriverManager>()
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IRegistrationPage, RegistrationPage>()
            .AddScoped<ISignInPage, SignInPage>()
            .AddScoped<ISuccessRegistrationPage, SuccessRegistrationPage>();

        return services;
    }
    
}
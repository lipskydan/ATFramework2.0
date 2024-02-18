namespace Demo;

public class DiSetup: DiSetupBase
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        CreateBaseServices(out var services);
        
        services
            .AddScoped<IHomePage, HomePage>()
            .AddScoped<IRegistrationPage, RegistrationPage>()
            .AddScoped<ISignInPage, SignInPage>()
            .AddScoped<ISuccessRegistrationPage, SuccessRegistrationPage>();

        return services;
    }
    
}



namespace NovaStream.Application.Services;

public static class ContainerRegistrationService
{
    public static void ApplicationRegister(this IServiceCollection services)
    {
        services.AddFluentValidation(options => options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
    }
}

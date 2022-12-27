namespace NovaStream.Infrastructure.Services;

public static class ContainerRegistrationService
{
    public static void InfrastructureRegister(this IServiceCollection services)
    {
        services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
        services.AddTransient<IPasswordEncryptorService, Sha256PasswordEncryptorService>();
    }
}

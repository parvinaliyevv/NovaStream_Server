namespace NovaStream.Infrastructure.Services;

public static class ContainerRegistrationService
{
    public static void InfrastructureRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonWebTokenOptions>(configuration.GetSection("JsonWebToken"));

        services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
        services.AddTransient<IEncryptorService, Sha256EncryptorService>();
    }
}

namespace NovaStream.Infrastructure.Services;

public static class ContainerRegistrationService
{
    public static void InfrastructureRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonWebTokenOptions>(configuration.GetSection("JsonWebToken"));
        services.Configure<AmazonWebServicesOptions>(configuration.GetSection("AmazonWebServices"));

        services.AddTransient<IStorageManager, AWSStorageManager>();
        services.AddTransient<IEncryptorService, Sha256EncryptorService>();
        services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
    }
}

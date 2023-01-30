namespace NovaStream.Infrastructure.Services;

public static class ContainerRegistrationService
{
    public static void InfrastructureRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JsonWebTokenOptions>(configuration.GetSection("JsonWebToken"));
        services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorage"));
        services.Configure<AmazonWebServicesOptions>(configuration.GetSection("AmazonWebServices"));
        services.Configure<SenderMailOptions>(configuration.GetSection("SenderMail"));

        services.AddTransient<IStorageManager, BlobStorageManager>();
        services.AddTransient<IAWSStorageManager, AWSStorageManager>();
        services.AddTransient<IEncryptorService, Sha256EncryptorService>();
        services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();
        services.AddTransient<IMailManager, MailManager>();
    }
}

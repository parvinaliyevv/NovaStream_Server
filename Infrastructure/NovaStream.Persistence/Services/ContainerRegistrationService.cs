namespace NovaStream.Persistence.Services;

public static class ContainerRegistrationService
{
    public static void PersistenceRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.
            GetConnectionString("DefaultConnection")));

        services.AddScoped<IUserManager, UserManager>();
    }
}

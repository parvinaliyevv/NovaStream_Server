namespace NovaStream.Application.Services;

public static class ContainerRegistrationService
{
    public static void ApplicationRegister(this IServiceCollection services)
    {
        services.AddFluentValidation(options => options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

        services.MapsterRegister();
    }

    private static void MapsterRegister(this IServiceCollection services)
    {
        var storageManager = services.BuildServiceProvider().GetRequiredService<IStorageManager>();
        var awsStorageManager = services.BuildServiceProvider().GetRequiredService<IAWSStorageManager>();

        _ = new SoonProfile(storageManager);
        _ = new MovieProfile(storageManager, awsStorageManager);
        _ = new GenreProfile(storageManager);
        _ = new ActorProfile(storageManager);
        _ = new SerialProfile(storageManager);
        _ = new EpisodeProfile(storageManager);
        _ = new ProducerProfile(storageManager);
    }
}

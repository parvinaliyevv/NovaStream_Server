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
        TypeAdapterConfig<Soon, SoonDto>
            .NewConfig()
            .Map(dest => dest.Day, src => src.OutDate.Day)
            .Map(dest => dest.Moon, src => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(src.OutDate.Month).ToUpper().Substring(0, 3));

        TypeAdapterConfig<Movie, MovieSearchDto>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.SearchImageUrl);

        TypeAdapterConfig<Serial, SerialSearchDto>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.SearchImageUrl);

        TypeAdapterConfig<Movie, MovieDto>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);

        TypeAdapterConfig<Serial, SerialDto>
            .NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);
    }
}

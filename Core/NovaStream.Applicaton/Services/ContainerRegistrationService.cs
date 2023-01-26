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
        TypeAdapterConfig<Movie, MovieSearchDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => src.SearchImageUrl);

        TypeAdapterConfig<Serial, SerialSearchDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => src.SearchImageUrl);

        TypeAdapterConfig<Movie, MovieDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);

        TypeAdapterConfig<Serial, SerialDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => src.ImageUrl);

        TypeAdapterConfig<Movie, MovieDetailsDto>.NewConfig()
            .Map(dest => dest.Actors, src => src.Actors.Select(ma => ma.Actor).Adapt<ICollection<ActorDto>>());

        TypeAdapterConfig<Serial, SerialDetailsDto>.NewConfig()
            .Map(dest => dest.Actors, src => src.Actors.Select(sa => sa.Actor).Adapt<ICollection<ActorDto>>());

        TypeAdapterConfig<Soon, SoonDto>.NewConfig()
            .Map(dest => dest.Day, src => src.OutDate.Day)
            .Map(dest => dest.Month, src => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(src.OutDate.Month).ToUpper().Substring(0, 3))
            .Map(dest => dest.Genres, src => Manufacturer.ManufactureGenres(src.Genres.Select(sc => sc.Genre.Name).ToList()));
    }
}

namespace NovaStream.Application.MapsterProfiles;

public class MovieProfile
{
	public MovieProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Movie, MovieDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Movie, MovieSearchDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.SearchImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Movie, MovieViewDetailsDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Movie, MovieDetailsDto>.NewConfig()
            .Map(dest => dest.VideoLength, src => Manufacturer.ManufactureTime(src.VideoLength))
            .Map(dest => dest.Actors, src => src.Actors.Select(ma => ma.Actor).Adapt<ICollection<ActorDto>>())
            .Map(dest => dest.VideoUrl, src => storageManager.GetSignedUrl(src.VideoUrl, TimeSpan.FromHours(7)))
            .Map(dest => dest.TrailerUrl, src => storageManager.GetSignedUrl(src.TrailerUrl, TimeSpan.FromHours(1)))
            .Map(dest => dest.VideoImageUrl, src => storageManager.GetSignedUrl(src.VideoImageUrl, TimeSpan.FromHours(1)));
    }
}

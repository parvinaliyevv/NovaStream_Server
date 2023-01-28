namespace NovaStream.Application.MapsterProfiles;

public class MovieProfile
{
	public MovieProfile(IStorageManager storageManager, IAWSStorageManager awsStorageManager)
	{
        TypeAdapterConfig<Movie, MovieDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl));

        TypeAdapterConfig<Movie, MovieSearchDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.SearchImageUrl));

        TypeAdapterConfig<Movie, MovieViewDetailsDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl));

        TypeAdapterConfig<Movie, MovieDetailsDto>.NewConfig()
            .Map(dest => dest.VideoLength, src => Manufacturer.ManufactureTime(src.VideoLength))
            .Map(dest => dest.Actors, src => src.Actors.Select(ma => ma.Actor).Adapt<ICollection<ActorDto>>())
            .Map(dest => dest.VideoUrl, src => awsStorageManager.GetSignedUrl(src.VideoUrl, TimeSpan.FromHours(7)))
            .Map(dest => dest.TrailerUrl, src => storageManager.GetSignedUrl(src.TrailerUrl))
            .Map(dest => dest.VideoImageUrl, src => storageManager.GetSignedUrl(src.VideoImageUrl));
    }
}

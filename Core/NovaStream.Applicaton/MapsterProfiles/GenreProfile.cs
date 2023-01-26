namespace NovaStream.Application.MapsterProfiles;

public class GenreProfile
{
	public GenreProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Genre, GenreDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));
    }
}

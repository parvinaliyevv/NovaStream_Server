namespace NovaStream.Application.MapsterProfiles;

public class DirectorProfile
{
	public DirectorProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Director, DirectorDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl));
    }
}

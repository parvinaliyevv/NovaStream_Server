namespace NovaStream.Application.MapsterProfiles;

public class EpisodeProfile
{
	public EpisodeProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Episode, EpisodeDto>.NewConfig()
            .Map(dest => dest.VideoLength, src => Manufacturer.ManufactureTime(src.VideoLength))
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl));
    }
}

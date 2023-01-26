namespace NovaStream.Application.MapsterProfiles;

public class ActorProfile
{
	public ActorProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Actor, ActorDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));
    }
}

namespace NovaStream.Application.MapsterProfiles;

public class ProducerProfile
{
	public ProducerProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Producer, ProducerDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl));
    }
}

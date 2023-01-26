namespace NovaStream.Application.MapsterProfiles;

public class SerialProfile
{
	public SerialProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Serial, SerialDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Serial, SerialSearchDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.SearchImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Serial, SerialViewDetailsDto>.NewConfig()
            .Map(dest => dest.ImageUrl, src => storageManager.GetSignedUrl(src.ImageUrl, TimeSpan.FromHours(1)));

        TypeAdapterConfig<Serial, SerialDetailsDto>.NewConfig()
            .Map(dest => dest.Actors, src => src.Actors.Select(ma => ma.Actor).Adapt<ICollection<ActorDto>>())
            .Map(dest => dest.TrailerUrl, src => storageManager.GetSignedUrl(src.TrailerUrl, TimeSpan.FromHours(1)));
    }
}

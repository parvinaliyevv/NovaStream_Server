namespace NovaStream.Application.MapsterProfiles;

public class SoonProfile
{
	public SoonProfile(IStorageManager storageManager)
	{
        TypeAdapterConfig<Soon, SoonDto>.NewConfig()
            .Map(dest => dest.Day, src => src.OutDate.Day)
            .Map(dest => dest.TrailerUrl, src => storageManager.GetSignedUrl(src.TrailerUrl, TimeSpan.FromHours(7)))
            .Map(dest => dest.TrailerImageUrl, src => storageManager.GetSignedUrl(src.TrailerImageUrl, TimeSpan.FromHours(1)))
            .Map(dest => dest.Month, src => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(src.OutDate.Month).ToUpper().Substring(0, 3))
            .Map(dest => dest.Genres, src => Manufacturer.ManufactureGenres(src.Genres.Select(sc => sc.Genre.Name).ToList()));
    }
}

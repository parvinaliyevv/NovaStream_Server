namespace NovaStream.Persistence.Data.Configurations;

public class SoonConfiguration : IEntityTypeConfiguration<Soon>
{
    public void Configure(EntityTypeBuilder<Soon> builder)
    {
        builder.HasKey(x => new { x.Name });

        var soons = new[]
        {
            new Soon()
            {
                Name = "John Wick: Chapter 4",
                Description = "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.",
                TrailerImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Soons%2FJohn%20Wick%3A%20Chapter%204%2Fjohn-wick-chapter-4-trailer-image.jpg?alt=media&token=410d8c6b-fe9c-4da4-a654-1980eb72c78e",
                TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Soons%2FJohn%20Wick%3A%20Chapter%204%2Fjohn-wick-chapter-4-trailer.mp4?alt=media&token=2c1e4eea-4a4f-4e00-add3-eb10b9dd5030",
                OutDate = DateTime.ParseExact("2023:03:24","yyyy:MM:dd", null)
            }
        };

        builder.HasData(soons);
    }
}

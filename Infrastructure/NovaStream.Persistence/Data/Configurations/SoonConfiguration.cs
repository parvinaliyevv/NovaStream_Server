namespace NovaStream.Persistence.Data.Configurations;

public class SoonConfiguration : IEntityTypeConfiguration<Soon>
{
    public void Configure(EntityTypeBuilder<Soon> builder)
    {
        builder.HasKey(s => s.Name);

        var soons = new Soon[]
        {
            new()
            {
                Name = "John Wick: Chapter 4",
                Description = "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.",
                TrailerImageUrl = @"Soons/John Wick: Chapter 4/john-wick-chapter-4-trailer-image.jpg",
                TrailerUrl = @"Soons/John Wick: Chapter 4/john-wick-chapter-4-trailer.mp4",
                OutDate = DateTime.ParseExact("2023:03:24","yyyy:MM:dd", null)
            },
            new()
            {
                Name = "Guardians of the Galaxy Vol. 3",
                Description = "Still reeling from the loss of Gamora, Peter Quill rallies his team to defend the universe and one of their own - a mission that could mean the end of the Guardians if not successful.",
                TrailerImageUrl = @"Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer-image.jpg",
                TrailerUrl = @"Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer.mp4",
                OutDate = DateTime.ParseExact("2023:05:05","yyyy:MM:dd", null)
            },
            new()
            {
                Name = "Transformers: Rise of the Beasts",
                Description = "Plot unknown. Reportedly based on the 'Transformers' spinoff 'Beast Wars' which feature robots that transform into robotic animals.",
                TrailerImageUrl = @"Soons/Transformers: Rise of the Beasts/transformers-rise-of-the-beasts-trailer-image.jpg",
                TrailerUrl = @"Soons/Transformers: Rise of the Beasts/transformers-rise-of-the-beasts-trailer.mp4",
                OutDate = DateTime.ParseExact("2023:06:09","yyyy:MM:dd", null)
            }
        };

        builder.HasData(soons);
    }
}

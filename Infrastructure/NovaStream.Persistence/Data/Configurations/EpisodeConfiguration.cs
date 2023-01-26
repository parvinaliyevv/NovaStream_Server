namespace NovaStream.Persistence.Data.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasOne(e => e.Season)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);

        var episodes = new Episode[]
        {
            new()
            {
                Id = 1,
                SeasonId = 1,
                Number = 1,
                VideoLength = TimeSpan.FromMinutes(80),
                Name = "1. When You're Lost in the Darkness",
                Description = "Twenty years after a fungal outbreak ravages the planet, survivors Joel and Ellie are tasked with a mission that could change everything.",
                ImageUrl = @"Serials/The Last of Us/Season 1/Episode 1/the-last-of-us-S01E01-video-image.jpg",
                VideoUrl = @"Serials/The Last of Us/Season 1/Episode 1/the-last-of-us-S01E01-video-720p.mkv"
            },
            new()
            {
                Id = 2,
                SeasonId = 1,
                Number = 2,
                VideoLength = TimeSpan.FromMinutes(52),
                Name = "2. Infected",
                Description = "Joel, Tess, and Ellie traverse through an abandoned and flooded Boston hotel on their way to drop Ellie off with a group of Fireflies.",
                ImageUrl = @"Serials/The Last of Us/Season 1/Episode 2/the-last-of-us-S01E02-video-image.jpg",
                VideoUrl = @"Serials/The Last of Us/Season 1/Episode 2/the-last-of-us-S01E02-video-720p.mkv"
            }
        };

        builder.HasData(episodes);
    }
}

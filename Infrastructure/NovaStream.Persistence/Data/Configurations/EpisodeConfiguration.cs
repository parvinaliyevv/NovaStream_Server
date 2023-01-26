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
                Name = "1. Episode 1",
                Description = "Ambitious gang leader Thomas Shelby recognizes an opportunity to move up in the world thanks to a missing crate of guns.",
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 1/peaky-blinders-S01E01-video-image",
                VideoUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 1/peaky-blinders-S01E01-video.mp4",
            },
            new()
            {
                Id = 2,
                SeasonId = 1,
                Number = 2,
                Name = "2. Episode 2",
                Description = "Thomas provokes a local kingpin by fixing a horse race and starts a war with a gypsy family; Inspector Campbell carries out a vicious raid.",
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 2/peaky-blinders-S01E02-video-image",
                VideoUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 2/peaky-blinders-S01E02-video.mp4",
            },
            new()
            {
                Id = 3,
                SeasonId = 2,
                Number = 1,
                Name = "1. Episode 1",
                Description = "When Birmingham crime boss Tommy Shelby's beloved Garrison pub is bombed, he finds himself blackmailed into murdering an Irish dissident.",
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 2/Episode 1/peaky-blinders-S02E01-video-image",
                VideoUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 2/Episode 1/peaky-blinders-S02E01-video.mp4",
            },
            new()
            {
                Id = 4,
                SeasonId = 3,
                Number = 1,
                Name = "1. Wednesday's Child Is Full of Woe",
                Description = "When a deliciously wicked prank gets Wednesday expelled, her parents ship her off to Nevermore Academy, the boarding school where they fell in love.",
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/Season 1/Episode 1/wednesday-S01E01-video-image",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E01-video.mkv?alt=media&token=ac3169c1-7e09-46f4-baa9-cad6e290244b"
            },
            new()
            {
                Id = 5,
                SeasonId = 3,
                Number = 2,
                Name = "2. Woe Is the Loneliest Number",
                Description = "The sheriff questions Wednesday about the night's strange happenings. Later, Wednesday faces off against a fierce rival in the cutthroat Poe Cup race.",
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/Season 1/Episode 2/wednesday-S01E02-video-image",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E02-video.mkv?alt=media&token=3fdc19c2-224e-4c06-afda-c7d5fd1e2db1"
            }
        };

        builder.HasData(episodes);
    }
}

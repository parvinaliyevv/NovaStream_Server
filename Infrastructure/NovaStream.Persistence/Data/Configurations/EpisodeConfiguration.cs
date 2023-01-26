namespace NovaStream.Persistence.Data.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasOne(e => e.Season)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);

        var episodes = new[]
        {
            new Episode()
            {
                Id = 1,
                SeasonId = 1,
                Number = 1,
                Name = "1. Episode 1",
                Description = "Ambitious gang leader Thomas Shelby recognizes an opportunity to move up in the world thanks to a missing crate of guns.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-image.jpg?alt=media&token=10d57b41-f838-4110-acf7-63f8c01abebf",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-video.mp4?alt=media&token=1ddc4b67-579d-4081-ae49-aea3c95b9402",
            },
            new Episode()
            {
                Id = 2,
                SeasonId = 1,
                Number = 2,
                Name = "2. Episode 2",
                Description = "Thomas provokes a local kingpin by fixing a horse race and starts a war with a gypsy family; Inspector Campbell carries out a vicious raid.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-image.jpg?alt=media&token=7e07da2d-90b7-443e-b72a-088ebf61e2a3",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-video.mp4?alt=media&token=c41cfa67-da90-4ee4-86af-7d5264617a69",
            },
            new Episode()
            {
                Id = 3,
                SeasonId = 2,
                Number = 1,
                Name = "1. Episode 1",
                Description = "When Birmingham crime boss Tommy Shelby's beloved Garrison pub is bombed, he finds himself blackmailed into murdering an Irish dissident.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-image.jpg?alt=media&token=acc1de8b-c971-41ee-9b0d-fff713ce484c",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-video.mp4?alt=media&token=1be97130-46ef-43d6-a195-c8e2e7d107d6",
            },
            new Episode()
            {
                Id = 4,
                SeasonId = 3,
                Number = 1,
                Name = "1. Wednesday's Child Is Full of Woe",
                Description = "When a deliciously wicked prank gets Wednesday expelled, her parents ship her off to Nevermore Academy, the boarding school where they fell in love.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2FEpisode%201%2Fwednesday-S01E01-image.jpg?alt=media&token=1a5b46ae-7cf3-46a0-994e-3b6072f36e43",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E01-video.mkv?alt=media&token=ac3169c1-7e09-46f4-baa9-cad6e290244b"
            },
            new Episode()
            {
                Id = 5,
                SeasonId = 3,
                Number = 2,
                Name = "2. Woe Is the Loneliest Number",
                Description = "The sheriff questions Wednesday about the night's strange happenings. Later, Wednesday faces off against a fierce rival in the cutthroat Poe Cup race.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2FEpisode%202%2Fwednesday-S01E02-image.jpg?alt=media&token=a387f24e-4dfb-4cd8-8381-1707b941ad2f",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E02-video.mkv?alt=media&token=3fdc19c2-224e-4c06-afda-c7d5fd1e2db1"
            }
        };

        builder.HasData(episodes);
    }
}

namespace NovaStream.Persistence.Data.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasOne(e => e.Season)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId);

        var episodes = new[]
        {
            new Episode()
            {
                Id = 1,
                SeasonId = 1,
                Number = 1,
                Name = "1. Episode 1",
                Description = "Thomas Shelby, leader of the Birmingham gang, the Peaky Blinders, comes into possession of a shipment of guns from the local BSA factory. Aware that keeping the guns could lead to trouble with the law, Thomas nonetheless wants to use the guns to increase the Peaky's power.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-image.jpg?alt=media&token=10d57b41-f838-4110-acf7-63f8c01abebf",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-video.mp4?alt=media&token=1ddc4b67-579d-4081-ae49-aea3c95b9402",
            },
            new Episode()
            {
                Id = 2,
                SeasonId = 1,
                Number = 2,
                Name = "1. Episode 2",
                Description = "Thomas Shelby sets out to get work with Billy Kimber - the man who can help Thomas achieve his dream of running a legal bookmaking business. Meanwhile, Polly is alarmed when she realizes Ada is pregnant and when Thomas discovers the news, he forces Ada to admit that Freddie Thorne is the father.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-image.jpg?alt=media&token=7e07da2d-90b7-443e-b72a-088ebf61e2a3",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-video.mp4?alt=media&token=c41cfa67-da90-4ee4-86af-7d5264617a69",
            },
            new Episode()
            {
                Id = 3,
                SeasonId = 2,
                Number = 1,
                Name = "2. Episode 1",
                Description = "As the 1920s begin to roar, business is booming for the Peaky Blinders gang. Tommy Shelby starts to expand his legal and illegal operations, with an eye on the racetracks of the south. Meanwhile, an enemy from Tommy's past returns to Birmingham.",
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-image.jpg?alt=media&token=acc1de8b-c971-41ee-9b0d-fff713ce484c",
                VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-video.mp4?alt=media&token=1be97130-46ef-43d6-a195-c8e2e7d107d6",
            },
        };

        builder.HasData(episodes);
    }
}

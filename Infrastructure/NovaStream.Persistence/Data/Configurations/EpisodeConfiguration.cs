namespace NovaStream.Persistence.Data.Configurations;

internal class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasOne(e => e.Season)
            .WithMany(s => s.Episodes)
            .HasForeignKey(e => e.SeasonId);

        var episodes = new[]
        {
            new Episode() { Id = 1, SeasonId = 1, Number = 1, Name = "Episode 1", Description = "Baza", ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E01.mp4?alt=media&token=728dbcd3-6215-4e76-ae69-6849ba9896f5" },
            new Episode() { Id = 2, SeasonId = 1, Number = 2, Name = "Episode 2", Description = "Baza", ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E02.mp4?alt=media&token=f2bcb626-2a22-4a69-bdb8-69fef93ca2c9" },
            new Episode() { Id = 3, SeasonId = 2, Number = 1, Name = "Episode 1", Description = "Baza", ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%202%2FPeaky-Blinders-S02E01.mp4?alt=media&token=740e66cd-9759-4b7b-8592-8a93919a059b" },
        };

        builder.HasData(episodes);
    }
}

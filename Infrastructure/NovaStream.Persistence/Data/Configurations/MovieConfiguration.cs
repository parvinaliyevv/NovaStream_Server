namespace NovaStream.Persistence.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(v => v.Name);

        builder.HasOne(m => m.Producer)
            .WithMany(p => p.Movies)
            .HasForeignKey(m => m.ProducerId)
            .OnDelete(DeleteBehavior.SetNull);

        var movie = new Movie()
        {
            Name = "Interstellar",
            Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.",
            Year = 2014,
            Age = 13,
            ProducerId = 2,
            VideoName = "Episode",
            VideoDescription = "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.",
            VideoUrl = @"https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-video",
            VideoImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-video-image",
            ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-image",
            SearchImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-search-image",
            TrailerUrl = @"https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-trailer"
        };

        builder.HasData(movie);
    }
}

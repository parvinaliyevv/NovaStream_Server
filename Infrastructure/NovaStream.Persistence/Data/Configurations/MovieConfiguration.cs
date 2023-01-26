namespace NovaStream.Persistence.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(v => new { v.Name });

        var movie = new Movie()
        {
            Name = "Interstellar",
            Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.",
            Year = 2014,
            Age = 13,
            VideoName = "Episode",
            VideoDescription = "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.",
            VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-video.mp4?alt=media&token=aa6a5dca-7570-45de-87ed-22d35d25189b",
            VideoImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-video-image.jpg?alt=media&token=5a18e02b-3976-4116-a591-d6e334c50772",
            ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-image.png?alt=media&token=776eca25-eae0-426e-b103-8ed2c47a0811",
            SearchImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-search-image.jpg?alt=media&token=5799d5f0-f87d-424c-9c62-b0ba5904f499",
            TrailerImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-trailer-image.jpg?alt=media&token=1da9a664-0a92-4e35-90d9-7905fd33dfdf",
            TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-trailer.mp4?alt=media&token=3e5984d4-5fb8-438b-aa9a-3778e3f52ba8"
        };

        builder.HasData(movie);
    }
}

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

        var movies = new Movie[]
        {
            new()
            {
                Name = "Interstellar",
                Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.",
                Year = 2014,
                Age = 13,
                ProducerId = 1,
                VideoLength = TimeSpan.FromMinutes(170),
                VideoName = "Interstellar",
                VideoDescription = "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.",
                VideoUrl = @"Movies/Interstellar/interstellar-video-720p.mp4",
                VideoImageUrl = @"Movies/Interstellar/interstellar-video-image.jpg",
                ImageUrl = @"Movies/Interstellar/interstellar-image.jpg",
                SearchImageUrl = @"Movies/Interstellar/interstellar-search-image.jpg",
                TrailerUrl = @"Movies/Interstellar/interstellar-trailer.mp4"
            },
            new()
            {
                Name = "Inception",
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
                Year = 2010,
                Age = 16,
                ProducerId = 1,
                VideoLength = TimeSpan.FromMinutes(148),
                VideoName = "Inception",
                VideoDescription = "A troubled thief who extracts secrets from people's dreams takes one last job: leading a dangerous mission to plant an idea in a target's subconscious.",
                VideoUrl = @"Movies/Inception/inception-video-720p.mkv",
                VideoImageUrl = @"Movies/Inception/inception-video-image.jpg",
                ImageUrl = @"Movies/Inception/inception-image.jpg",
                SearchImageUrl = @"Movies/Inception/inception-search-image.jpg",
                TrailerUrl = @"Movies/Inception/inception-trailer.mp4"
            }
        };

        builder.HasData(movies);
    }
}

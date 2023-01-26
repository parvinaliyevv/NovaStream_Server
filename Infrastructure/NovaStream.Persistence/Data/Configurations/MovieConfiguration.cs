namespace NovaStream.Persistence.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(v => new { v.Name });

        var movie = new Movie()
        {
            Name = "Interstellar",
            Description = "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.",
            Year = 2014,
            Age = 13,
            ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FInterstellar%2FInterstellar.png?alt=media&token=0a6c45f5-fc92-4b66-8d50-7222ae8815b8",
            VideoUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar.mp4?alt=media&token=0fbe924c-8746-4132-8440-f8331d5214f6",
            TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar%20Trailer.mp4?alt=media&token=562d9a36-a3b2-411b-9525-05ccdc65e11a"
        };

        builder.HasData(movie);
    }
}

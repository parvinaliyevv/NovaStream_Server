namespace NovaStream.Persistence.Data.Configurations;

public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(sc => new { sc.MovieName, sc.GenreId });

        builder.HasOne(bc => bc.Movie)
            .WithMany(b => b.Genres)
            .HasForeignKey(bc => bc.MovieName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bc => bc.Genre)
            .WithMany(c => c.MovieGenres)
            .HasForeignKey(bc => bc.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        var movieGenres = new MovieGenre[]
        {
            new() { GenreId = 1, MovieName = "Interstellar" },
            new() { GenreId = 2, MovieName = "Interstellar" },
            new() { GenreId = 3, MovieName = "Interstellar" },
            new() { GenreId = 2, MovieName = "Inception" },
            new() { GenreId = 3, MovieName = "Inception" },
            new() { GenreId = 4, MovieName = "Inception" },
            new() { GenreId = 6, MovieName = "Inception" }
        };

        builder.HasData(movieGenres);
    }
}

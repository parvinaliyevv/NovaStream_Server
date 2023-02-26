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
    }
}

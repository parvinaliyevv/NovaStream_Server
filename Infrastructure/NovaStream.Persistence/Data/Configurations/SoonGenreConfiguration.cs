namespace NovaStream.Persistence.Data.Configurations;

public class SoonGenreConfiguration : IEntityTypeConfiguration<SoonGenre>
{
    public void Configure(EntityTypeBuilder<SoonGenre> builder)
    {
        builder.HasKey(icc => new { icc.SoonName, icc.GenreId });

        builder.HasOne(icc => icc.Soon)
            .WithMany(ic => ic.Genres)
            .HasForeignKey(icc => icc.SoonName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(icc => icc.Genre)
            .WithMany(c => c.SoonGenres)
            .HasForeignKey(c => c.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        var soonGenres = new SoonGenre[]
        {
            new() { GenreId = 2, SoonName = "John Wick: Chapter 4" },
            new() { GenreId = 7, SoonName = "John Wick: Chapter 4" },
            new() { GenreId = 8, SoonName = "John Wick: Chapter 4" },
            new() { GenreId = 7, SoonName = "Guardians of the Galaxy Vol. 3" },
            new() { GenreId = 4, SoonName = "Guardians of the Galaxy Vol. 3" },
            new() { GenreId = 12, SoonName = "Guardians of the Galaxy Vol. 3" },
            new() { GenreId = 7, SoonName = "Transformers: Rise of the Beasts" },
            new() { GenreId = 4, SoonName = "Transformers: Rise of the Beasts" },
            new() { GenreId = 13, SoonName = "Transformers: Rise of the Beasts" }
        };

        builder.HasData(soonGenres);
    }
}

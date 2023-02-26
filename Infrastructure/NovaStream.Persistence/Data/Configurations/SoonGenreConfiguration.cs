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
    }
}

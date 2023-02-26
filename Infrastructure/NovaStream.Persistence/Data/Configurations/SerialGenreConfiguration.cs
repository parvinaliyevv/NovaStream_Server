namespace NovaStream.Persistence.Data.Configurations;

public class SerialGenreConfiguration : IEntityTypeConfiguration<SerialGenre>
{
    public void Configure(EntityTypeBuilder<SerialGenre> builder)
    {
        builder.HasKey(sc => new { sc.SerialName, sc.GenreId });

        builder.HasOne(bc => bc.Serial)
            .WithMany(b => b.Genres)
            .HasForeignKey(bc => bc.SerialName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bc => bc.Genre)
            .WithMany(c => c.SerialGenres)
            .HasForeignKey(bc => bc.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

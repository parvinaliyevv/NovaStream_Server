namespace NovaStream.Persistence.Data.Configurations;

public class MovieMarkConfiguration : IEntityTypeConfiguration<MovieMark>
{
    public void Configure(EntityTypeBuilder<MovieMark> builder)
    {
        builder.HasKey(mm => new { mm.MovieName, mm.UserEmail });

        builder
            .HasOne(mm => mm.Movie)
            .WithMany(m => m.Marks)
            .HasForeignKey(mm => mm.MovieName)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(mm => mm.User)
            .WithMany(u => u.MovieMarks)
            .HasForeignKey(mm => mm.UserEmail)
            .OnDelete(DeleteBehavior.Cascade);

        var movieMark = new MovieMark("admin@novastream.api", "Interstellar");

        builder.HasData(movieMark);
    }
}

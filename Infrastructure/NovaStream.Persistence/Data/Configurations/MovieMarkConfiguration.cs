namespace NovaStream.Persistence.Data.Configurations;

public class MovieMarkConfiguration : IEntityTypeConfiguration<MovieMark>
{
    public void Configure(EntityTypeBuilder<MovieMark> builder)
    {
        builder.HasKey(mm => new { mm.MovieName, mm.UserId });

        builder.HasOne(mm => mm.Movie)
            .WithMany(m => m.Marks)
            .HasForeignKey(mm => mm.MovieName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mm => mm.User)
            .WithMany(u => u.MovieMarks)
            .HasForeignKey(mm => mm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        var movieMark = new MovieMark("Interstellar", 1);

        builder.HasData(movieMark);
    }
}

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
    }
}

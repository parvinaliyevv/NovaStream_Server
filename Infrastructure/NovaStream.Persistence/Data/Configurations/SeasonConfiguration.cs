namespace NovaStream.Persistence.Data.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasOne(s => s.Serial)
            .WithMany(v => v.Seasons)
            .HasForeignKey(s => s.SerialName)
            .OnDelete(DeleteBehavior.Cascade);

        var seasons = new Season[]
        {
            new() { Id = 1, Number = 1, SerialName = "The Last of Us" },
        };

        builder.HasData(seasons);
    }
}

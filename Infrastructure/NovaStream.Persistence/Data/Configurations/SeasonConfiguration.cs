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
            new() { Id = 1, Number = 1, SerialName = "Peaky Blinders" },
            new() { Id = 2, Number = 2, SerialName = "Peaky Blinders" },
            new() { Id = 3, Number = 1, SerialName = "Wednesday" }
        };

        builder.HasData(seasons);
    }
}

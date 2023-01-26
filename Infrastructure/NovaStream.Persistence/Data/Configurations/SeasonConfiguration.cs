namespace NovaStream.Persistence.Data.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasOne(s => s.Serial)
            .WithMany(v => v.Seasons)
            .HasForeignKey(s => s.SerialName);

        var seasons = new[]
        {
            new Season() { Id = 1, SerialName = "Peaky Blinders", Number = 1 },
            new Season() { Id = 2, SerialName = "Peaky Blinders", Number = 2 }
        };

        builder.HasData(seasons);
    }
}

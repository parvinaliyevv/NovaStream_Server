namespace NovaStream.Persistence.Data.Configurations;

public class SerialCategoryConfiguration : IEntityTypeConfiguration<SerialCategory>
{
    public void Configure(EntityTypeBuilder<SerialCategory> builder)
    {
        builder.HasKey(sc => new { sc.SerialName, sc.CategoryId });

        builder
            .HasOne(bc => bc.Serial)
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.SerialName);

        builder
            .HasOne(bc => bc.Category)
            .WithMany(c => c.SerialCategories)
            .HasForeignKey(bc => bc.CategoryId);

        var serialCategories = new List<SerialCategory>()
        {
            new() { SerialName="Peaky Blinders", CategoryId=1 },
            new() { SerialName="Peaky Blinders", CategoryId=2 },
            new() { SerialName="Peaky Blinders", CategoryId=3 }
        };

        builder.HasData(serialCategories);
    }
}

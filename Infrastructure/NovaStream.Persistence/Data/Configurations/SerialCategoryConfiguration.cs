namespace NovaStream.Persistence.Data.Configurations;

public class SerialCategoryConfiguration : IEntityTypeConfiguration<SerialCategory>
{
    public void Configure(EntityTypeBuilder<SerialCategory> builder)
    {
        builder.HasKey(sc => new { sc.SerialName, sc.CategoryId });

        builder.HasOne(bc => bc.Serial)
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.SerialName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.SerialCategories)
            .HasForeignKey(bc => bc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        var serialCategories = new[]
        {
            new SerialCategory() { SerialName = "Peaky Blinders", CategoryId = 1 },
            new SerialCategory() { SerialName = "Peaky Blinders", CategoryId = 2 },
            new SerialCategory() { SerialName = "Peaky Blinders", CategoryId = 3 },
            new SerialCategory() { SerialName = "Wednesday", CategoryId = 9 },
            new SerialCategory() { SerialName = "Wednesday", CategoryId = 10 },
            new SerialCategory() { SerialName = "Wednesday", CategoryId = 11 }
        };

        builder.HasData(serialCategories);
    }
}

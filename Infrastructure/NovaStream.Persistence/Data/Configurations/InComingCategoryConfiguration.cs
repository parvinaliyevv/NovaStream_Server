namespace NovaStream.Persistence.Data.Configurations;

public class InComingCategoryConfiguration : IEntityTypeConfiguration<InComingCategory>
{
    public void Configure(EntityTypeBuilder<InComingCategory> builder)
    {
        builder.HasKey(icc => new { icc.InComingVideoName, icc.CategoryId });

        builder
            .HasOne(icc => icc.InComingVideo)
            .WithMany(ic => ic.Categories)
            .HasForeignKey(icc => icc.InComingVideoName);

        builder
            .HasOne(icc => icc.Category)
            .WithMany(c => c.InComingCategories)
            .HasForeignKey(c => c.CategoryId);

        var inComingCategories = new List<InComingCategory>()
        {
            new InComingCategory() { CategoryId = 1, InComingVideoName = "Sahmaran" },
            new InComingCategory() { CategoryId = 3, InComingVideoName = "Sahmaran" },
            new InComingCategory() { CategoryId = 5, InComingVideoName = "Sahmaran" },
        };

        builder.HasData(inComingCategories);
    }
}

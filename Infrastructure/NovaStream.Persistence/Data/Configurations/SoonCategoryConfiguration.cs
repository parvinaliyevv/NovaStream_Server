namespace NovaStream.Persistence.Data.Configurations;

public class SoonCategoryConfiguration : IEntityTypeConfiguration<SoonCategory>
{
    public void Configure(EntityTypeBuilder<SoonCategory> builder)
    {
        builder.HasKey(icc => new { icc.SoonName, icc.CategoryId });

        builder.HasOne(icc => icc.Soon)
            .WithMany(ic => ic.Categories)
            .HasForeignKey(icc => icc.SoonName);

        builder.HasOne(icc => icc.Category)
            .WithMany(c => c.InComingCategories)
            .HasForeignKey(c => c.CategoryId);

        var soonCategories = new[]
        {
            new SoonCategory() { CategoryId = 2, SoonName = "John Wick: Chapter 4" },
            new SoonCategory() { CategoryId = 7, SoonName = "John Wick: Chapter 4" },
            new SoonCategory() { CategoryId = 8, SoonName = "John Wick: Chapter 4" },
        };

        builder.HasData(soonCategories);
    }
}

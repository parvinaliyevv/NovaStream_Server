namespace NovaStream.Persistence.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var categories = new List<Category>()
        {
            new() { Id=1, Name="Drama" },
            new() { Id=2, Name="Crime" },
            new() { Id=3, Name="Historical" },
            new() { Id=4, Name="Adventure" },
            new() { Id=5, Name="Science fiction" },
            new() { Id=6, Name="Detective" }
        };

        builder.HasData(categories);
    }
}

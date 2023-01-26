namespace NovaStream.Persistence.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var categories = new[]
        {
            new Category() { Id = 1, Name = "Drama" },
            new Category() { Id = 2, Name = "Crime" },
            new Category() { Id = 3, Name = "Historical" },
            new Category() { Id = 4, Name = "Adventure" },
            new Category() { Id = 5, Name = "Science fiction" },
            new Category() { Id = 6, Name = "Detective" },
            new Category() { Id = 7, Name = "Action" },
            new Category() { Id = 8, Name = "Thriller" },
        };

        builder.HasData(categories);
    }
}

namespace NovaStream.Persistence.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        var genres = new Genre[]
        {
            new() { Id = 1, Name = "Drama", ImageUrl = @"Images/Genres/drama-image.png" },
            new() { Id = 2, Name = "Adventure", ImageUrl = @"Images/Genres/adventure-image.png" },
            new() { Id = 3, Name = "Sci-Fi", ImageUrl = @"Images/Genres/sci-fi-image.png" },
            new() { Id = 4, Name = "Action", ImageUrl = @"Images/Genres/action-image.png" },
            new() { Id = 5, Name = "Horror", ImageUrl = @"Images/Genres/horror-image.png" },
            new() { Id = 6, Name = "Thriller", ImageUrl = @"Images/Genres/thriller-image.png" },
            new() { Id = 7, Name = "Crime", ImageUrl = @"Images/Genres/crime-image.png" },
            new() { Id = 8, Name = "Comedy", ImageUrl = @"Images/Genres/comedy-image.png" } 
        };

        builder.HasData(genres);
    }
}

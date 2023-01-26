namespace NovaStream.Persistence.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        var genres = new Genre[]
        {
            new() { Id = 1, Name = "Drama", ImageUrl = @"baza" },
            new() { Id = 2, Name = "Crime", ImageUrl = @"baza" },
            new() { Id = 3, Name = "Historical", ImageUrl = @"baza" },
            new() { Id = 4, Name = "Adventure", ImageUrl = @"baza" },
            new() { Id = 5, Name = "Science fiction", ImageUrl = @"baza" },
            new() { Id = 6, Name = "Detective", ImageUrl = @"baza" },
            new() { Id = 7, Name = "Action", ImageUrl = @"baza" },
            new() { Id = 8, Name = "Thriller", ImageUrl = @"baza" },
            new() { Id = 9, Name = "Comedy horror", ImageUrl = @"baza" },
            new() { Id = 10, Name = "Coming-of-age", ImageUrl = @"baza" },
            new() { Id = 11, Name = "Supernatural", ImageUrl = @"baza" },
            new() { Id = 12, Name = "Comedy", ImageUrl = @"baza" },
            new() { Id = 13, Name = "Sci-Fi", ImageUrl = @"baza" },
        };

        builder.HasData(genres);
    }
}

﻿namespace NovaStream.Persistence.Data.Configurations;

public class MovieCategoryConfiguration : IEntityTypeConfiguration<MovieCategory>
{
    public void Configure(EntityTypeBuilder<MovieCategory> builder)
    {
        builder.HasKey(sc => new { sc.MovieName, sc.CategoryId });

        builder.HasOne(bc => bc.Movie)
            .WithMany(b => b.Categories)
            .HasForeignKey(bc => bc.MovieName);

        builder.HasOne(bc => bc.Category)
            .WithMany(c => c.MovieCategories)
            .HasForeignKey(bc => bc.CategoryId);

        var movieCategories = new[]
        {
            new MovieCategory() { MovieName = "Interstellar", CategoryId = 1 },
            new MovieCategory() { MovieName = "Interstellar", CategoryId = 4 },
            new MovieCategory() { MovieName = "Interstellar", CategoryId = 5 },
            new MovieCategory() { MovieName = "Interstellar", CategoryId = 6 }
        };

        builder.HasData(movieCategories);
    }
}
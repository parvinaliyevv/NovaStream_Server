﻿namespace NovaStream.Persistence.Data.Configurations;

public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
{
    public void Configure(EntityTypeBuilder<MovieActor> builder)
    {
        builder.HasKey(ma => new { ma.MovieName, ma.ActorId });

        builder.HasOne(ma => ma.Movie)
            .WithMany(m => m.Actors)
            .HasForeignKey(ma => ma.MovieName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ma => ma.Actor)
            .WithMany(a => a.Movies)
            .HasForeignKey(ma => ma.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        var movieActors = new MovieActor[]
        {
            new() { ActorId = 2, MovieName = "Interstellar" },
            new() { ActorId = 3, MovieName = "Interstellar" }
        };

        builder.HasData(movieActors);
    }
}

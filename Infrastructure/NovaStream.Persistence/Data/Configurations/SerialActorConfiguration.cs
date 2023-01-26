namespace NovaStream.Persistence.Data.Configurations;

public class SerialActorConfiguration : IEntityTypeConfiguration<SerialActor>
{
    public void Configure(EntityTypeBuilder<SerialActor> builder)
    {
        builder.HasKey(sa => new { sa.SerialName, sa.ActorId });

        builder.HasOne(sa => sa.Serial)
            .WithMany(s => s.Actors)
            .HasForeignKey(sa => sa.SerialName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.Actor)
            .WithMany(a => a.Serials)
            .HasForeignKey(sa => sa.ActorId)
            .OnDelete(DeleteBehavior.Cascade);

        var serialActors = new SerialActor[]
        {
            new() { ActorId = 4, SerialName = "The Last of Us" },
            new() { ActorId = 5, SerialName = "The Last of Us" },
            new() { ActorId = 6, SerialName = "The Last of Us" }
        };

        builder.HasData(serialActors);
    }
}

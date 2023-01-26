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
            new() { ActorId = 1, SerialName = "Peaky Blinders" },
            new() { ActorId = 2, SerialName = "Peaky Blinders" }
        };

        builder.HasData(serialActors);
    }
}

namespace NovaStream.Persistence.Data.Configurations;

public class SerialConfiguration : IEntityTypeConfiguration<Serial>
{
    public void Configure(EntityTypeBuilder<Serial> builder)
    {
        builder.HasKey(v => v.Name);

        builder.HasOne(s => s.Producer)
            .WithMany(p => p.Serials)
            .HasForeignKey(s => s.ProducerId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

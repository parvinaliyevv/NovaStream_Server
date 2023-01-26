namespace NovaStream.Persistence.Data.Configurations;

public class SerialMarkConfiguration : IEntityTypeConfiguration<SerialMark>
{
    public void Configure(EntityTypeBuilder<SerialMark> builder)
    {
        builder.HasKey(sm => new { sm.SerialName, sm.UserEmail });

        builder
            .HasOne(sm => sm.Serial)
            .WithMany(s => s.SerialMarks)
            .HasForeignKey(sm => sm.SerialName);

        builder
            .HasOne(sm => sm.User)
            .WithMany(u => u.SerialMarks)
            .HasForeignKey(sm => sm.UserEmail);
    }
}

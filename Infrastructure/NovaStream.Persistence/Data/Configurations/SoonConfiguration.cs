namespace NovaStream.Persistence.Data.Configurations;

public class SoonConfiguration : IEntityTypeConfiguration<Soon>
{
    public void Configure(EntityTypeBuilder<Soon> builder)
    {
        builder.HasKey(s => s.Name);
    }
}

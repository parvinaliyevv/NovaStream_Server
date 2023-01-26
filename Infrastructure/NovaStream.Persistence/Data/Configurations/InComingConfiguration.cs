namespace NovaStream.Persistence.Data.Configurations;

public class InComingConfiguration : IEntityTypeConfiguration<InComing>
{
    public void Configure(EntityTypeBuilder<InComing> builder)
    {
        builder.HasKey(x => new { x.Name });

        var inComings = new List<InComing>()
        {
            new InComing()
            {
                Name = "Sahmaran",
                OutDate = DateTime.Now,
                Description = "DATABASES",
                TrailerUrl = "videobaza"
            }
        };

        builder.HasData(inComings);
    }
}

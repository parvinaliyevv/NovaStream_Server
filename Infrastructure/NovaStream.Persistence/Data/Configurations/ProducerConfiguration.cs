namespace NovaStream.Persistence.Data.Configurations;

public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> builder)
    {
        builder.HasKey(p => new { p.Id });

        var producers = new[]
        {
            new Producer { Id = 1, Name = "Murad", Surname = "Musayev", About = "zor oglan" },
            new Producer { Id = 2, Name = "Parvin", Surname = "Aliyev", About = "zor oglan" },
            new Producer { Id = 3, Name = "Rustem", Surname = "Bayramov", About = "zor oglan" }
        };

        builder.HasData(producers);
    }
}

namespace NovaStream.Persistence.Data.Configurations;

public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> builder)
    {
        var producers = new Producer[]
        {
            new() { Id = 1, Name = "Murad", Surname = "Musayev", About = "zor oglan", ImageUrl = "baza" },
            new() { Id = 2, Name = "Parvin", Surname = "Aliyev", About = "zor oglan", ImageUrl = "baza" },
            new() { Id = 3, Name = "Rustem", Surname = "Bayramov", About = "zor oglan", ImageUrl = "baza" }
        };

        builder.HasData(producers);
    }
}

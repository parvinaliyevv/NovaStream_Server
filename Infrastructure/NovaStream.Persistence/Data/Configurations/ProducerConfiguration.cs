namespace NovaStream.Persistence.Data.Configurations;

public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> builder)
    {
        var producers = new Producer[]
        {
            new()
            {
                Id = 1,
                Name = "Christopher",
                Surname = "Nolan",
                ImageUrl = @"Images/Producers/Christopher-Nolan-image.jpg",
                About = "Best known for his cerebral, often nonlinear, storytelling, acclaimed writer-director Christopher Nolan was born on July 30, 1970, in London, England. Over the course of 15 years of filmmaking, Nolan has gone from low-budget independent films to working on some of the biggest blockbusters ever made."
            },
            new()
            {
                Id = 2,
                Name = "Craig",
                Surname = "Mazin",
                ImageUrl = @"Images/Producers/Craig-Mazin-image.jpg",
                About = "Craig Mazin was born on April 8, 1971 in Brooklyn, New York, USA. He is a producer and writer, known for Chernobyl (2019), The Hangover Part II (2011) and Identity Thief (2013)."
            },
        };

        builder.HasData(producers);
    }
}

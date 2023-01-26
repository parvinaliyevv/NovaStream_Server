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

        var serials = new Serial[]
        {
            new()
            {
                Name = "Peaky Blinders",
                Description = "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.",
                Year = 2013,
                Age = 18,
                ProducerId = 1,
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-image",
                SearchImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-search-image",
                TrailerUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-trailer"
            },
            new()
            {
                Name = "Wednesday",
                Description = "Follows Wednesday Addams' years as a student, when she attempts to master her emerging psychic ability, thwart and solve the mystery that embroiled her parents.",
                Year = 2022,
                Age = 14,
                ProducerId = 3,
                ImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-image",
                SearchImageUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-search-image",
                TrailerUrl = @"https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-trailer"
            }
        };

        builder.HasData(serials);
    }
}

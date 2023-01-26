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
                Name = "The Last of Us",
                Description = "After a global pandemic destroys civilization, a hardened survivor takes charge of a 14-year-old girl, who may be humanity's last hope.",
                Year = 2023,
                Age = 16,
                ProducerId = 2,
                ImageUrl = @"Serials/The Last of Us/the-last-of-us-image.jpg",
                SearchImageUrl = @"Serials/The Last of Us/the-last-of-us-search-image.jpg",
                TrailerUrl = @"Serials/The Last of Us/the-last-of-us-trailer.mp4"
            }
        };

        builder.HasData(serials);
    }
}

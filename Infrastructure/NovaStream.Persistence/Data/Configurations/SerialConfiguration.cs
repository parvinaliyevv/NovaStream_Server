namespace NovaStream.Persistence.Data.Configurations;

public class SerialConfiguration : IEntityTypeConfiguration<Serial>
{
    public void Configure(EntityTypeBuilder<Serial> builder)
    {
        builder.HasKey(v => new { v.Name });

        builder.HasOne(s => s.Producer)
            .WithMany(p => p.Serials)
            .HasForeignKey(s => s.ProducerId);

        var serials = new[]
        {
            new Serial()
            {
                Name = "Peaky Blinders",
                Description = "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.",
                Year = 2013,
                Age = 18,
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-image.jpg?alt=media&token=356b23bd-755e-4daf-822e-50a029c87f9c",
                SearchImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-search-image.jpg?alt=media&token=8ea5abb6-b969-4bf4-a20a-13ffcd3a07fd",
                TrailerImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-trailer-image.jpg?alt=media&token=a99966d3-1793-4cac-97fc-80b9b75686f0",
                TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-trailer.mp4?alt=media&token=c5e7aef9-cfcf-4a31-8e77-8c678d95bd7b",
                ProducerId = 1
            },
            new Serial()
            {
                Name = "Wednesday",
                Description = "Follows Wednesday Addams' years as a student, when she attempts to master her emerging psychic ability, thwart and solve the mystery that embroiled her parents.",
                Year = 2022,
                Age = 14,
                ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2Fwednesday-image.jpg?alt=media&token=54703ac5-97e4-48fb-929b-3b7c57759764",
                SearchImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2Fwednesday-search-image.jpg?alt=media&token=ea516245-d4a0-4649-8c67-68bc39cbda9b",
                TrailerImageUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2Fwednesday-trailer-image.jpg?alt=media&token=6763d2c7-2994-46ff-9d61-45956081e0d4",
                TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2Fwednesday-trailer.mp4?alt=media&token=cd22789d-43ef-477d-bf5d-442adee817f0",
                ProducerId = 3
            }
        };

        builder.HasData(serials);
    }
}

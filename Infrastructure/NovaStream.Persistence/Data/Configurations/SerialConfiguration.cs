namespace NovaStream.Persistence.Data.Configurations;

public class SerialConfiguration : IEntityTypeConfiguration<Serial>
{
    public void Configure(EntityTypeBuilder<Serial> builder)
    {
        builder.HasKey(v => new { v.Name });

        var serial = new Serial()
        {
            Name = "Peaky Blinders",
            Description = "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.",
            Year = 2013,
            Age = 18,
            ImageUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988",
            TrailerUrl = @"https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky%20Blinder%20Season%201%20Trailer.mp4?alt=media&token=ce9640c0-b5ea-4f6f-9d1d-11b5a518ab32"
        };

        builder.HasData(serial);
    }
}

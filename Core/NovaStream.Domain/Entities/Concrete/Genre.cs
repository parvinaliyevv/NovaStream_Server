namespace NovaStream.Domain.Entities.Concrete;

public class Genre : Entity
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<MovieGenre> MovieGenres { get; set; }
    public ICollection<SerialGenre> SerialGenres { get; set; }
    public ICollection<SoonGenre> SoonGenres { get; set; }
}

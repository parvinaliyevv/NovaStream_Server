namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class MovieGenre
{
    public Movie Movie { get; set; }
    public Genre Genre { get; set; }
    public int GenreId { get; set; }
    public string MovieName { get; set; }
}

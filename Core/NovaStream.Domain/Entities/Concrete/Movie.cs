namespace NovaStream.Domain.Entities.Concrete;

public class Movie : Video
{
    public string VideoPath { get; set; }
    public ICollection<MovieCategory> MovieCategories { get; set; }
}

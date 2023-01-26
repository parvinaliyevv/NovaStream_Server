namespace NovaStream.Domain.Entities.Concrete;

public class Movie : Video
{
    public string VideoUrl { get; set; }

    public ICollection<MovieMark> Marks { get; set; }
    public ICollection<MovieCategory> Categories { get; set; }
}

namespace NovaStream.Domain.Entities.Concrete;

public class Movie : Video
{
    public string VideoName { get; set; }
    public string VideoDescription { get; set; }
    public string VideoImageUrl { get; set; }
    public string VideoUrl { get; set; }

    public ICollection<MovieMark> Marks { get; set; }
    public ICollection<MovieCategory> Categories { get; set; }
}

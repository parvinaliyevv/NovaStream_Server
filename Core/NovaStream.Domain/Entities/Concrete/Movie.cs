namespace NovaStream.Domain.Entities.Concrete;

public class Movie : Video
{
    public string VideoUrl { get; set; }
    public string VideoName { get; set; }
    public string VideoImageUrl { get; set; }
    public string VideoDescription { get; set; }
    public TimeSpan VideoLength { get; set; }

    public int? ProducerId { get; set; }
    public Producer? Producer { get; set; }

    public ICollection<MovieMark> Marks { get; set; }
    public ICollection<MovieActor> Actors { get; set; }
    public ICollection<MovieGenre> Genres { get; set; }
}

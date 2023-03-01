namespace NovaStream.Domain.Entities.Concrete;

public class Serial : Video
{
    public int? DirectorId { get; set; }
    public Director? Director { get; set; }

    public ICollection<Season> Seasons { get; set; }
    public ICollection<SerialMark> Marks { get; set; }
    public ICollection<SerialActor> Actors { get; set; }
    public ICollection<SerialGenre> Genres { get; set; }
}

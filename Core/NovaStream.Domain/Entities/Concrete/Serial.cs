namespace NovaStream.Domain.Entities.Concrete;

public class Serial : Video
{
    public ICollection<Season> Seasons { get; set; }
    public ICollection<SerialCategory> SerialCategories { get; set; }

    public ICollection<SerialMark> SerialMarks { get; set; }
}

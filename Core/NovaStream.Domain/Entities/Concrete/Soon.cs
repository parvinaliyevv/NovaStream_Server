namespace NovaStream.Domain.Entities.Concrete;

public class Soon
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TrailerUrl { get; set; }
    public string TrailerImageUrl { get; set; }
    public DateTime OutDate { get; set; }
    
    public ICollection<SoonGenre> Genres { get; set; }
}

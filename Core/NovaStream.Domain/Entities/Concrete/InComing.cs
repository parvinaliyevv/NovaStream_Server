namespace NovaStream.Domain.Entities.Concrete;

public class InComing
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TrailerUrl { get; set; }
    public DateTime OutDate { get; set; }
    
    public ICollection<InComingCategory> Categories { get; set; }
}

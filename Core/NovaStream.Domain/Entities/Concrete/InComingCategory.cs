namespace NovaStream.Domain.Entities.Concrete;

public class InComingCategory
{
    public string InComingVideoName { get; set; }
    public InComing InComingVideo { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}

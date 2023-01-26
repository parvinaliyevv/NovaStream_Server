namespace NovaStream.Domain.Entities.Concrete;

public class SoonCategory
{
    public string SoonName { get; set; }
    public Soon Soon { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}

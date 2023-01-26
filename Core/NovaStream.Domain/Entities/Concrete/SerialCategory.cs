namespace NovaStream.Domain.Entities.Concrete;

public class SerialCategory
{
    public string SerialName { get; set; }
    public Serial Serial { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}

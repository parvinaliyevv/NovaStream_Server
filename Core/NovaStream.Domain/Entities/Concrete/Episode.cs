namespace NovaStream.Domain.Entities.Concrete;

public class Episode : Entity
{
    public int Number { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string VideoUrl { get; set; }

    public int SeasonId { get; set; }
    public Season Season { get; set; }
}

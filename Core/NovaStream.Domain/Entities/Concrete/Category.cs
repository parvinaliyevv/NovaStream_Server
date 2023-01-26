namespace NovaStream.Domain.Entities.Concrete;

public class Category : Entity
{
    public string Name { get; set; }

    public ICollection<MovieCategory> MovieCategories { get; set; }
    public ICollection<SerialCategory> SerialCategories { get; set; }
    public ICollection<SoonCategory> InComingCategories { get; set; }
}

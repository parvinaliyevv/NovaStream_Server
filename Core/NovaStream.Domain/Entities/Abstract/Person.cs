namespace NovaStream.Domain.Entities.Abstract;

public abstract class Person : Entity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string About { get; set; }
    public string ImageUrl { get; set; }
}

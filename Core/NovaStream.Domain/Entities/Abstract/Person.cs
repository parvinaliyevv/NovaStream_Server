namespace NovaStream.Domain.Entities.Abstract;

public abstract class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string About { get; set; }
    public string? ImageUrl { get; set; }
}

namespace NovaStream.Domain.Entities.Concrete;

public class Director : Person
{
    public ICollection<Movie> Movies { get; set; }
    public ICollection<Serial> Serials { get; set; }
}

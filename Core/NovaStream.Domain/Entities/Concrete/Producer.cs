namespace NovaStream.Domain.Entities.Concrete;

public class Producer : Person
{
    public ICollection<Movie> Movies { get; set; }
    public ICollection<Serial> Serials { get; set; }
}

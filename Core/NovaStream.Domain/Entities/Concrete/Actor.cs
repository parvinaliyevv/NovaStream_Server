namespace NovaStream.Domain.Entities.Concrete;

public class Actor : Person 
{
    public ICollection<MovieActor> Movies { get; set; }
    public ICollection<SerialActor> Serials { get; set; }
}


namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class MovieActor
{
    public Movie? Movie { get; set; }    
    public string MovieName { get; set; }
    public Actor? Actor { get; set; }
    public int ActorId { get; set; }
}

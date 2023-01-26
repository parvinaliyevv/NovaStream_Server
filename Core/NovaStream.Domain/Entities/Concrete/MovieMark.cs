namespace NovaStream.Domain.Entities.Concrete;

public class MovieMark
{
    public string MovieName { get; set; }
    public Movie Movie { get; set; }
    public string UserEmail { get; set; }
    public User User { get; set; }
}

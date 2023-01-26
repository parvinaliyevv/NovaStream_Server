namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class MovieMark
{
    public string? MovieName { get; set; }
    public Movie? Movie { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }


    public MovieMark(string? movieName, int? userId)
    {
        MovieName = movieName;
        UserId = userId;
    }
}

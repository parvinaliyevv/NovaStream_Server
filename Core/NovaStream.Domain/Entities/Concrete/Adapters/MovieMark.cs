namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class MovieMark
{
    public Movie Movie { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public string MovieName { get; set; }


    public MovieMark(int userId, string movieName)
    {
        UserId = userId;
        MovieName = movieName;
    }
}

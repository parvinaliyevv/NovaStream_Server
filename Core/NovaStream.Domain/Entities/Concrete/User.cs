namespace NovaStream.Domain.Entities.Concrete;

public class User
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<MovieMark> MovieMarks { get; set; }
    public ICollection<SerialMark> SerialMarks { get; set; }
}

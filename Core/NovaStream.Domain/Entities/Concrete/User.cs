namespace NovaStream.Domain.Entities.Concrete;

public class User: Entity
{
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string AvatarUrl { get; set; }
    public string PasswordHash { get; set; }
    public string OldPasswordHash { get; set; }
    public string ConfirmationPIN { get; set; }

    public ICollection<MovieMark> MovieMarks { get; set; }
    public ICollection<SerialMark> SerialMarks { get; set; }
}

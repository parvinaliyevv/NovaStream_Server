namespace NovaStream.Domain.Entities.Concrete;

public class SerialMark
{
    public string SerialName { get; set; }
    public Serial Serial { get; set; }
    public string UserEmail { get; set; }
    public User User { get; set; }
}

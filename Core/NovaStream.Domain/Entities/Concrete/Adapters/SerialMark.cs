namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class SerialMark
{
    public string? SerialName { get; set; }
    public Serial? Serial { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }


    public SerialMark(string? serialName, int? userId)
    {
        SerialName = serialName;
        UserId = userId;
    }
}

namespace NovaStream.Domain.Entities.Concrete.Adapters;

public class SerialMark
{
    public Serial Serial { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public string SerialName { get; set; }


    public SerialMark(int userId, string serialName)
    {
        UserId = userId;
        SerialName = serialName;
    }
}

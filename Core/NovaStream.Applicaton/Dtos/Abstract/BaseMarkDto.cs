namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseMarkDto
{
    public string Name { get; set; }
    public bool IsSerial { get; set; }
}

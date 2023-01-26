namespace NovaStream.Application.Dtos.Abstract;

public abstract record BaseUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

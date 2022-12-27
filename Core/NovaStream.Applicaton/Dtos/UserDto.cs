namespace NovaStream.Application.Dtos;

public abstract record BaseUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public record SignInUserDto(): BaseUserDto;

public record SignUpUserDto(): BaseUserDto;

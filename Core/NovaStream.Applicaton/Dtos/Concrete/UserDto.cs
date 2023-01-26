namespace NovaStream.Application.Dtos.Concrete;

public record SignInUserDto(): BaseUserDto;

public record SignUpUserDto(string Nickname, string AvatarUrl) : BaseUserDto;

public record EditUserDto(string Nickname, string AvatarUrl);

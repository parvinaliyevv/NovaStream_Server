namespace NovaStream.Application.Services;

public interface ITokenGeneratorService
{
    string GenerateToken(User user);
    Task<string> GenerateTokenAsync(User user);
}

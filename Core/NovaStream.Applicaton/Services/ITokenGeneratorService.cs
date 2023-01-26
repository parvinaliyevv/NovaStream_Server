namespace NovaStream.Application.Services;

public interface ITokenGeneratorService
{
    string GenerateAuthorizeToken(User user);
    Task<string> GenerateAuthorizeTokenAsync(User user);
}

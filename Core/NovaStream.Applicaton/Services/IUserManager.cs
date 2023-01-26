namespace NovaStream.Application.Services;

public interface IUserManager
{
    bool CreateUser(User user, string password);
    Task<bool> CreateUserAsync(User user, string password);

    User? FindUserByEmail(string email);
    Task<User?> FindUserByEmailAsync(string email);

    User? ReturnUserFromContext(HttpContext httpContext);
    Task<User?> ReturnUserFromContextAsync(HttpContext httpContext);

    bool CheckPassword(User user, string password);
    Task<bool> CheckPasswordAsync(User user, string password);

    bool Exists(string email);
    Task<bool> ExistsAsync(string email);
}

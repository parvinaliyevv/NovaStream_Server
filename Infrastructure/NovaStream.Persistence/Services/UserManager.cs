namespace NovaStream.Persistence.Services;

public class UserManager : IUserManager
{
    private readonly AppDbContext _dbContext;

    private readonly IPasswordEncryptorService _passwordEncryptorService;


    public UserManager(AppDbContext dbContext, IPasswordEncryptorService passwordEncryptorService)
    {
        _dbContext = dbContext;

        _passwordEncryptorService = passwordEncryptorService;
    }


    public bool CheckPassword(User user, string password)
    {
        var encryptedPassword = _passwordEncryptorService.Encrypt(password);

        return user.PasswordHash == encryptedPassword;
    }
    public async Task<bool> CheckPasswordAsync(User user, string password)
        => await Task.Factory.StartNew(() => CheckPassword(user, password));

    public bool ChangePassword(User user, string newPassword)
    {
        user.PasswordHash = _passwordEncryptorService.Encrypt(newPassword);

        return UpdateUser(user);
    }
    public async Task<bool> ChangePasswordAsync(User user, string newPassword)
        => await Task.Factory.StartNew(() => ChangePassword(user, newPassword));

    public bool CreateUser(User user, string password)
    {
        user.PasswordHash = _passwordEncryptorService.Encrypt(password);

        var entry = _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return entry.State == EntityState.Unchanged;
    }
    public async Task<bool> CreateUserAsync(User user, string password)
        => await Task.Factory.StartNew(() => CreateUser(user, password));

    public bool UpdateUser(User user)
    {
        var entry = _dbContext.Users.Update(user);

        _dbContext.SaveChanges();

        return entry.State == EntityState.Modified;
    }
    public async Task<bool> UpdateUserAsync(User user)
        => await Task.Factory.StartNew(() => UpdateUser(user));

    public User? ReturnUserFromContext(HttpContext httpContext)
    {
        var identity = httpContext.User.Identity as ClaimsIdentity;

        if (identity is null) return null;

        var userClaims = identity.Claims;
        var idClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (idClaim is null) return null;

        var userId = int.Parse(idClaim);

        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);

        return user;
    }
    public async Task<User?> ReturnUserFromContextAsync(HttpContext httpContext)
        => await Task.Factory.StartNew(() => ReturnUserFromContext(httpContext));

    public bool Exists(string email)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Email == email);

        return user is not null;
    }
    public async Task<bool> ExistsAsync(string email)
        => await Task.Factory.StartNew(() => Exists(email));

    public User? FindUserByEmail(string email)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Email == email);

        return user;
    }
    public async Task<User?> FindUserByEmailAsync(string email)
        => await Task.Factory.StartNew(() => FindUserByEmail(email));
}

namespace NovaStream.Persistence.Services;

public class UserManager : IUserManager
{
    private readonly AppDbContext _dbContext;

    private readonly IEncryptorService _passwordEncryptorService;


    public UserManager(AppDbContext dbContext, IEncryptorService passwordEncryptorService)
    {
        _dbContext = dbContext;

        _passwordEncryptorService = passwordEncryptorService;
    }


    public bool CreateUser(User user, string password)
    {
        user.PasswordHash = _passwordEncryptorService.EncryptPassword(password);
        user.OldPasswordHash = user.PasswordHash;

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

        return entry.State == EntityState.Unchanged;
    }
    public async Task<bool> UpdateUserAsync(User user)
        => await Task.Factory.StartNew(() => UpdateUser(user));

    public bool DeleteUser(User user)
    {
        var entry = _dbContext.Users.Remove(user);

        _dbContext.SaveChanges();

        return entry.State == EntityState.Detached;
    }
    public async Task<bool> DeleteUserAsync(User user)
        => await Task.Factory.StartNew(() => DeleteUser(user));
    
    public User? FindUserByEmail(string email)
    {
        var user = _dbContext.Users.FirstOrDefault(user => user.Email == email);

        return user;
    }
    public async Task<User?> FindUserByEmailAsync(string email)
        => await Task.Factory.StartNew(() => FindUserByEmail(email));

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
    
    public bool CheckPassword(User user, string password)
    {
        var encryptedPassword = _passwordEncryptorService.EncryptPassword(password);

        return user.PasswordHash == encryptedPassword;
    }
    public async Task<bool> CheckPasswordAsync(User user, string password)
        => await Task.Factory.StartNew(() => CheckPassword(user, password));

    public bool CheckOldPassword(User user, string password)
    {
        var encryptedPassword = _passwordEncryptorService.EncryptPassword(password);

        return user.OldPasswordHash == encryptedPassword;
    }
    public async Task<bool> CheckOldPasswordAsync(User user, string password)
        => await Task.Factory.StartNew(() => CheckPassword(user, password));

    public bool ChangePassword(User user, string newPassword)
    {
        user.OldPasswordHash = user.PasswordHash;
        user.PasswordHash = _passwordEncryptorService.EncryptPassword(newPassword);

        return UpdateUser(user);
    }
    public async Task<bool> ChangePasswordAsync(User user, string newPassword)
        => await Task.Factory.StartNew(() => ChangePassword(user, newPassword));

    public bool Exists(string email) => _dbContext.Users.Any(user => user.Email == email);
    public async Task<bool> ExistsAsync(string email) => await Task.Factory.StartNew(() => Exists(email));
}

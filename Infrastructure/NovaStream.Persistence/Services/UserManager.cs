namespace NovaStream.Persistence.Services;

public class UserManager: IUserManager
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

    public bool CreateUser(User user, string password)
    {
        user.PasswordHash = _passwordEncryptorService.Encrypt(password);

        var entry = _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return entry.State == EntityState.Unchanged;
    }
    public async Task<bool> CreateUserAsync(User user, string password)
        => await Task.Factory.StartNew(() => CreateUser(user, password));

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

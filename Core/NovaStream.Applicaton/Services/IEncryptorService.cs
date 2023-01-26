namespace NovaStream.Application.Services;

public interface IEncryptorService
{
    string EncryptPassword(string password);
    Task<string> EncryptPasswordAsync(string password);
}

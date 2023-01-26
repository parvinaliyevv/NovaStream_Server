namespace NovaStream.Infrastructure.Services;

public class Sha256EncryptorService : IEncryptorService
{
    public string EncryptPassword(string password)
    {
        using var encryptor = SHA256.Create();
        var builder = new StringBuilder();

        var bytes = encryptor.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));

        return builder.ToString();
    }

    public async Task<string> EncryptPasswordAsync(string password)
        => await Task.Factory.StartNew(() => EncryptPassword(password));
}

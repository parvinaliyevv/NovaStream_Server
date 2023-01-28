namespace NovaStream.Application.Services;

public interface IAWSStorageManager
{
    string GetSignedUrl(string filename, TimeSpan expireTime);
    Task<string> GetSignedUrlAsync(string filename, TimeSpan expireTime);

    // string UploadFile(IFormFile file, string filename);
    // Task<string> UploadFileAsync(IFormFile file, string filename);
}

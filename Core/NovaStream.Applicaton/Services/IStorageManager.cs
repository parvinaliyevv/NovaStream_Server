namespace NovaStream.Application.Services;

public interface IStorageManager
{
    string GetSignedUrl(string filename);
    Task<string> GetSignedUrlAsync(string filename);

    // string UploadFile(IFormFile file, string filename);
    // Task<string> UploadFileAsync(IFormFile file, string filename);
}

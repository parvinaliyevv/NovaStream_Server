namespace NovaStream.Application.Services;

public interface IAWSStorageManager
{
    string GetSignedUrl(string filename, TimeSpan expireTime);
    Task<string> GetSignedUrlAsync(string filename, TimeSpan expireTime);

    Task<bool> DeleteFileAsync(string filename);

    void UploadFile(FileStream stream, string filename, EventHandler<UploadProgressArgs> progress);
    Task UploadFileAsync(FileStream stream, string filename, EventHandler<UploadProgressArgs> progress, CancellationToken cancellationToken);
}

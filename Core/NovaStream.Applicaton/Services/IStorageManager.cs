namespace NovaStream.Application.Services;

public interface IStorageManager
{
    string GetSignedUrl(string filename);
    Task<string> GetSignedUrlAsync(string filename);

    bool DeleteFile(string filename);
    Task<bool> DeleteFileAsync(string filename);

    void UploadFile(FileStream stream, string filename, IProgress<long> progress);
    Task UploadFileAsync(FileStream stream, string filename, IProgress<long> progress, CancellationToken cancellationToken);
}

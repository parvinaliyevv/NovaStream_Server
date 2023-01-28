namespace NovaStream.Infrastructure.Services;

public class BlobStorageManager : IStorageManager
{
    private readonly BlobStorageOptions _storageOptions;


    public BlobStorageManager(IOptions<BlobStorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }


    public string GetSignedUrl(string filename)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(filename);

        return blobClient.Uri.AbsoluteUri;
    }
    public async Task<string> GetSignedUrlAsync(string filename)
        => await Task.Factory.StartNew(() => GetSignedUrl(filename));
}

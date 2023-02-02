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

    public bool DeleteFile(string filename)
    {
        var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(filename);

        return blobClient.DeleteIfExists();
    }
    public Task<bool> DeleteFileAsync(string filename)
        => Task.Factory.StartNew(() => DeleteFile(filename));

    public void UploadFile(FileStream stream, string filename, IProgress<long> progress)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(filename);

        var blobUploadOptions = new BlobUploadOptions()
        {
            ProgressHandler = progress
        };

        blobClient.Upload(stream, blobUploadOptions);
    }
    public async Task UploadFileAsync(FileStream stream, string filename, IProgress<long> progress, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var containerClient = new BlobContainerClient(_storageOptions.ConnectionString, _storageOptions.ContainerName);
        var blobClient = containerClient.GetBlobClient(filename);
        
        var blobUploadOptions = new BlobUploadOptions()
        {
            ProgressHandler = progress,
            TransferOptions = new Azure.Storage.StorageTransferOptions() { MaximumTransferSize = 123_456_789 }
        };

        await blobClient.UploadAsync(stream, blobUploadOptions, cancellationToken);
    }
}

namespace NovaStream.Infrastructure.Services;

public class AWSStorageManager : IAWSStorageManager
{
    private readonly AmazonWebServicesOptions _storageOptions;


    public AWSStorageManager(IOptions<AmazonWebServicesOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }


    public string GetSignedUrl(string filename, TimeSpan expireTime)
    {
        var credentials = new BasicAWSCredentials(_storageOptions.AccessKey, _storageOptions.SecretKey);
        var client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(_storageOptions.Region));

        var request = new GetPreSignedUrlRequest()
        {
            Key = filename,
            Expires = DateTime.Now.Add(expireTime),
            BucketName = _storageOptions.BucketName
        };

        var url = client.GetPreSignedURL(request);

        return url;
    }
    public Task<string> GetSignedUrlAsync(string filename, TimeSpan expireTime)
        => Task.Factory.StartNew(() => GetSignedUrl(filename, expireTime));

    public async Task<bool> DeleteFileAsync(string filename)
    {
        var credentials = new BasicAWSCredentials(_storageOptions.AccessKey, _storageOptions.SecretKey);
        var client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(_storageOptions.Region));

        var request = new DeleteObjectRequest()
        {
             BucketName = _storageOptions.BucketName,
             Key = filename
        };

        var result =  await client.DeleteObjectAsync(request);

        return result.HttpStatusCode == HttpStatusCode.OK;
    }

    public void UploadFile(FileStream stream, string filename, EventHandler<UploadProgressArgs> progress)
    {
        var credentials = new BasicAWSCredentials(_storageOptions.AccessKey, _storageOptions.SecretKey);
        var client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(_storageOptions.Region));

        var uploadRequest = new TransferUtilityUploadRequest()
        {
            InputStream = stream,
            BucketName =  _storageOptions.BucketName,
            Key = filename
        };

        uploadRequest.UploadProgressEvent += progress;

        var fileTransferUtility = new TransferUtility(client);
        fileTransferUtility.Upload(uploadRequest);
    }
    public async Task UploadFileAsync(FileStream stream, string filename, EventHandler<UploadProgressArgs> progress, CancellationToken cancellationToken)
    {
        var credentials = new BasicAWSCredentials(_storageOptions.AccessKey, _storageOptions.SecretKey);
        var client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(_storageOptions.Region));

        var uploadRequest = new TransferUtilityUploadRequest()
        {
            InputStream = stream,
            BucketName = _storageOptions.BucketName,
            Key = filename
        };

        uploadRequest.UploadProgressEvent += progress;
        
        var fileTransferUtility = new TransferUtility(client);
        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);
    }
}

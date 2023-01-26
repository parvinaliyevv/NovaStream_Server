namespace NovaStream.Infrastructure.Services;

public class AWSStorageManager : IStorageManager
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
}

using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Ecommerce_api.Infrastructure.FileStorage;

public class S3FileStorage : IFileStorage
{
    private readonly IAmazonS3 _s3;
    private readonly AwsSettings _awsSettings;

    public S3FileStorage(
        IAmazonS3 s3,
        IOptions<AwsSettings> awsSettings)
    {
        _s3 = s3;
        _awsSettings = awsSettings.Value;
    }

    public async Task<string> UploadAsync(
        Stream stream,
        string key,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest()
        {
            BucketName = _awsSettings.S3.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType
        };

        await _s3.PutObjectAsync(request, cancellationToken);

        return key;
    }

    public async Task DeleteAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        await _s3.DeleteObjectAsync(
            _awsSettings.S3.BucketName,
            key,
            cancellationToken);
    }
}
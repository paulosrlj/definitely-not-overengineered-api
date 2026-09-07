namespace Ecommerce_api.Infrastructure.FileStorage;

public class AwsSettings
{
    public S3Settings S3 { get; set; } = null!;
}

public class S3Settings
{
    public string BucketName { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string CloudFrontDomain { get; set; } = string.Empty;
    public string? ServiceUrl { get; set; }
}

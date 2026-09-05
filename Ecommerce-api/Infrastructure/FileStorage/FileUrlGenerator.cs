using Microsoft.Extensions.Options;

namespace Ecommerce_api.Infrastructure.FileStorage;

public class CloudFrontFileUrlGenerator : IFileUrlGenerator
{
    private readonly AwsSettings _settings;

    public CloudFrontFileUrlGenerator(IOptions<AwsSettings> settings)
    {
        _settings = settings.Value;
    }

    public string Generate(string storageKey)
    {
        return $"{_settings.S3.CloudFrontDomain.TrimEnd('/')}/{storageKey}";
    }
}
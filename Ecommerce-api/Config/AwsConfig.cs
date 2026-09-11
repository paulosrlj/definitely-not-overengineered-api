using Amazon.Runtime;
using Amazon.S3;
using Ecommerce_api.Infrastructure.FileStorage;

namespace Ecommerce_api.Config;

public static class AwsConfig
{
    public static IServiceCollection AddAwsService(this IServiceCollection services, IConfiguration configuration)
    {
        var awsSettings = configuration.GetSection("AWS").Get<AwsSettings>();
        services.Configure<AwsSettings>(configuration.GetSection("AWS"));

        var credentials = new BasicAWSCredentials(awsSettings!.S3.AccessKey, awsSettings.S3.SecretKey);
        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1,
            };

            // Floci
            // Configurei meu AWS pra usar o floci em ambiente DEV
            if (!string.IsNullOrEmpty(awsSettings.S3.ServiceUrl))
            {
                config.ServiceURL = awsSettings.S3.ServiceUrl;
                config.ForcePathStyle = true;
                config.UseHttp = true;
            }
    
            return new AmazonS3Client(credentials, config);
        });
        
        // builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWS:S3"));
        // builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        // builder.Services.AddAWSService<IAmazonS3>();
        
        return services;
    }
}
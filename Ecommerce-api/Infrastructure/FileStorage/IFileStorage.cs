namespace Ecommerce_api.Infrastructure.FileStorage;

public interface IFileStorage
{
    Task<string> UploadAsync(
        Stream stream,
        string key,
        string contentType,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string key,
        CancellationToken cancellationToken = default);
}
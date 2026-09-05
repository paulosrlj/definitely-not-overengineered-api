namespace Ecommerce_api.Infrastructure.FileStorage;

public interface IFileUrlGenerator
{
    string Generate(string storageKey);
}
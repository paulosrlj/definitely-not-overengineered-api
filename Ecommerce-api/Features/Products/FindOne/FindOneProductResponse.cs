using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Products.FindOne;

public record FindOneProductResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock,
    List<ProductImageResponse> Images
);

public record ProductImageResponse(
    int Id,
    string FileName,
    string ContentType,
    string StorageKey,
    int SortOrder
);
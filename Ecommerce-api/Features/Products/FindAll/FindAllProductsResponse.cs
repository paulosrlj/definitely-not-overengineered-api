using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Products;

namespace Ecommerce_api.Features.Products.FindAll;

public record ProductFileResponse(
    int Id,
    string FileName,
    string ContentType,
    long Size,
    string Url,
    int SortOrder
);

public record FindAllProductsResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock,
    ICollection<Category> Categories,
    ICollection<ProductFileResponse> Files
);
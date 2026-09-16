using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Products.Search;

public record SearchProductFileResponse(
    int Id,
    string FileName,
    string ContentType,
    long Size,
    string Url,
    int SortOrder
);

public record SearchProductsResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock,
    ICollection<Category> Categories,
    ICollection<SearchProductFileResponse> Files
);
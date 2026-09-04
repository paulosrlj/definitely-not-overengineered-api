namespace Ecommerce_api.Features.Products.Create;

public record CreateProductResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock,
    IReadOnlyCollection<string> Images
);
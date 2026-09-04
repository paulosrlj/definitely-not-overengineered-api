namespace Ecommerce_api.Features.Products.Update;

public record UpdateProductResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock
);
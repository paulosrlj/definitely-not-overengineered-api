using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Products;

namespace Ecommerce_api.Features.Products.FindAll;

public record FindAllProductsResponse(
    int Id,
    string Name,
    string Description,
    int Price,
    int Stock,
    ICollection<Category> Categories,
    ICollection<ProductFile> Files)
{
public static FindAllProductsResponse FromEntity(Product product)
{
    return new FindAllProductsResponse(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Stock,
        product.Categories,
        product.Files
    );
}
}

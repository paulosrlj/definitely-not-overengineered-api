using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Products.Update;

public class UpdateProductHandler
{
    private readonly AppDbContext _context;

    public UpdateProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateProductResponse?> Handle(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);

        if (product is null)
            return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock
        );
    }
}
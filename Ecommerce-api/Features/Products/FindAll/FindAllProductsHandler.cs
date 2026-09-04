using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Products.FindAll;

public class FindAllProductsHandler
{
    private readonly AppDbContext _context;

    public FindAllProductsHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FindAllProductsResponse>> Handle(
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Select(product => new FindAllProductsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.Categories,
                product.Files
            ))
            .ToListAsync(cancellationToken);
    }
}

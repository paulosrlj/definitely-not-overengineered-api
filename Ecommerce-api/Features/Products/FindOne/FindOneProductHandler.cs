using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Products.FindOne;

public class FindOneProductHandler
{
    private readonly AppDbContext _context;

    public FindOneProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FindOneProductResponse?> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(product => new FindOneProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.Files
                    .OrderBy(file => file.SortOrder)
                    .Select(file => new ProductImageResponse(
                        file.FileId,
                        file.File.FileName,
                        file.File.ContentType,
                        file.File.StorageKey,
                        file.SortOrder
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
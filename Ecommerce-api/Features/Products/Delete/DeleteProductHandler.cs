using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Ecommerce_api.Infrastructure.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Products.Delete;

public class DeleteProductHandler
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;

    public DeleteProductHandler(
        AppDbContext context,
        IFileStorage fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Files)
            .ThenInclude(pf => pf.File)
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);

        if (product is null)
            return false;

        foreach (var productFile in product.Files)
        {
            await _fileStorage.DeleteAsync(
                productFile.File.StorageKey,
                cancellationToken);
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}


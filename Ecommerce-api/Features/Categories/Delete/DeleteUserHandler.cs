using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Categories.Delete;

public class DeleteCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public DeleteCategoryHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"category:{id}";
        var category = await _cache.GetAsync<Category>(cacheKey);

        if (category is null)
        {
            category = await _context.Categories
                .FirstOrDefaultAsync(
                    c => c.Id == id,
                    cancellationToken) ?? throw new NotFoundException("Category not found");
        }
        
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);
    }
}


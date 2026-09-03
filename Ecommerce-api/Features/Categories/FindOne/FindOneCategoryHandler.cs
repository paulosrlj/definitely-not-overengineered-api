using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Categories.FindOne;

public class FindOneCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public FindOneCategoryHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<FindOneCategoryResponse> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{id}";
        var cached = await _cache.GetAsync<Category>(cacheKey);

        if (cached is not null)
            return FindOneCategoryResponse.FromEntity(cached);
        

        var category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                   ?? throw new NotFoundException("Category");
        
        await _cache.SetAsync(cacheKey, category, TimeSpan.FromMinutes(1));
        
        return FindOneCategoryResponse.FromEntity(category);
    }
}
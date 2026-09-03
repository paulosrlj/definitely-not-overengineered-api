using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Categories.Update;

public class UpdateCategoryHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public UpdateCategoryHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<UpdateCategoryResponse> Handle(
        int id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"category:{id}";
        var category = await _cache.GetAsync<Category>(cacheKey);

        if (category is null)
        {
            category = await _context.Categories
                .FirstOrDefaultAsync(
                    cat => cat.Id == id,
                    cancellationToken);

            if (category is null)
                throw new NotFoundException("Category");
        }

        if (request.Name is not null)
            category.Name = request.Name;

        if (request.Description is not null)
        {
            category.Description = request.Description;
        }

        if (request.IconIdentifier is not null)
        {
            category.IconIdentifier = request.IconIdentifier;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(cacheKey);

        return UpdateCategoryResponse.FromEntity(category);
    }
}
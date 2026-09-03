using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Categories.FindAll;

public class FindAllCategoriesHandler
{
    private readonly AppDbContext _context;

    public FindAllCategoriesHandler(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<PaginatedResponse<FindAllCategoriesResponse>> Handle(
        FindAllCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.Limit;
        var query = _context.Categories.AsNoTracking().AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{request.Search}%"));
        }

        var total = await query.CountAsync(cancellationToken);
        var categories = await query.Skip(skip).Take(request.Limit).ToListAsync(cancellationToken);
        
        return new PaginatedResponse<FindAllCategoriesResponse>(
            categories.Select(FindAllCategoriesResponse.FromEntity).ToList(), total, request.Page, request.Limit); 
    }
}


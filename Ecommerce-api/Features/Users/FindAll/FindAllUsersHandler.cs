using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Features.Users.FindAll;

public class FindAllUsersHandler
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public FindAllUsersHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<PaginatedResponse<FindAllUsersResponse>> Handle(
        FindAllUsersRequest request,
        CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.Limit;
        var query = _context.Users.AsNoTracking().AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{request.Search}%") ||
                EF.Functions.ILike(u.Email, $"%{request.Search}%"));
        }

        var total = await query.CountAsync(cancellationToken);
        var users = await query.Skip(skip).Take(request.Limit).ToListAsync(cancellationToken);
        
        return new PaginatedResponse<FindAllUsersResponse>(
            users.Select(FindAllUsersResponse.FromEntity).ToList(), total, request.Page, request.Limit); 
    }
}


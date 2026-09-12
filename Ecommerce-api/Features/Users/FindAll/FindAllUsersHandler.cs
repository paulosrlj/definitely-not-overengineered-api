using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Users.FindAll;

public class FindAllUsersHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<FindAllUsersHandler> _logger;

    public FindAllUsersHandler(AppDbContext dbContext,  ILogger<FindAllUsersHandler> logger)
    {
        _context = dbContext;
        _logger = logger;
    }

    public async Task<PaginatedResponse<FindAllUsersResponse>> Handle(
        FindAllUsersRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("FindAllUsers request received");
        var skip = (request.Page - 1) * request.Limit;
        var query = _context.Users.AsNoTracking().AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{request.Search}%") ||
                EF.Functions.ILike(u.Email, $"%{request.Search}%"));
        }

        var total = await query.CountAsync(cancellationToken);
        var users = await query
            .OrderBy(u => u.Id)
            .Skip(skip)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);
        
        return new PaginatedResponse<FindAllUsersResponse>(
            users.Select(FindAllUsersResponse.FromEntity).ToList(), total, request.Page, request.Limit);
    }
}


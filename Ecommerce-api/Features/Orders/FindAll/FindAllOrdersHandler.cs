using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Orders.FindAll;

public class FindAllOrdersHandler
{
    private readonly AppDbContext _context;

    public FindAllOrdersHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResponse<FindAllOrdersResponse>> Handle(
        FindAllOrdersRequest request,
        CancellationToken cancellationToken
    )
    {
        var skip = (request.Page - 1) * request.Limit;
        var query = _context.Orders.AsNoTracking().AsQueryable();

        var total = await query.CountAsync(cancellationToken);
        var orders = await query
            .Include(order => order.Payments)
            .Include(order => order.Items)
            .Skip(skip)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        var ordersResponse = FindAllOrdersResponse.ToResponse(orders);

        return new PaginatedResponse<FindAllOrdersResponse>(
            ordersResponse,
            total,
            request.Page,
            request.Limit
        );
    }
}
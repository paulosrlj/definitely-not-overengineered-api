using Ecommerce_api.Data;
using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Orders.FindOne;

public class FindOneOrderHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public FindOneOrderHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<FindOneOrderResponse> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"order:{id}";
        var cached = await _cache.GetAsync<FindOneOrderResponse>(cacheKey);

        if (cached is not null)
            return cached;

        var order = await _context.Orders
            .Where(order => order.Id == id)
            .Include(order => order.Payments)
            .Include(order => order.Items)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (order is null)
            throw new NotFoundException("Order");
        
        var response = FindOneOrderResponse.ToResponse(order);

        await _cache.SetAsync(
            cacheKey,
            response,
            TimeSpan.FromMinutes(1)
        );

        return response;
    }
}
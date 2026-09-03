using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Users.FindOne;

public class FindOneUserHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public FindOneUserHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<FindOneUserResponse> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{id}";
        var cached = await _cache.GetAsync<User>(cacheKey);

        if (cached is not null)
            return FindOneUserResponse.FromEntity(cached);
        

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                   ?? throw new NotFoundException("User");
        
        await _cache.SetAsync(cacheKey, user, TimeSpan.FromMinutes(1));
        
        return FindOneUserResponse.FromEntity(user);
    }
}
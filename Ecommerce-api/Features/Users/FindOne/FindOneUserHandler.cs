using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Features.Users.FindOne;

public class FindOneUserHandler
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public FindOneUserHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<FindOneUserResponse> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"user:{id}";

        if (_cache.TryGetValue<User>(cacheKey, out var cachedUser) && cachedUser is not null)
            return FindOneUserResponse.FromEntity(cachedUser);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
                   ?? throw new NotFoundException("User");
        
        _cache.Set(cacheKey, user, TimeSpan.FromMinutes(1));
        return FindOneUserResponse.FromEntity(user);
    }
}
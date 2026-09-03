using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Users.Delete;

public class DeleteUserHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public DeleteUserHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{id}";
        var user = await _cache.GetAsync<User>(cacheKey);

        if (user is null)
        {
            user = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Id == id,
                    cancellationToken) ?? throw new NotFoundException("User not found");
        }
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);
    }
}


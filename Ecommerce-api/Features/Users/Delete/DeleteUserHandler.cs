using Ecommerce_api.Data;
using Ecommerce_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Features.Users.Delete;

public class DeleteUserHandler
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;

    public DeleteUserHandler(AppDbContext dbContext, IMemoryCache cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                user => user.Id == id,
                cancellationToken) ?? throw new NotFoundException("User not found");
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        _cache.Remove($"user:{id}");
    }
}


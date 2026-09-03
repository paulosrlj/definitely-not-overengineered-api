using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Features.Users.Update;

public class UpdateUserHandler
{
    private readonly AppDbContext _context;
    private readonly ICacheService _cache;

    public UpdateUserHandler(AppDbContext dbContext, ICacheService cache)
    {
        _context = dbContext;
        _cache = cache;
    }

    public async Task<UpdateUserResponse> Handle(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{id}";
        var user = await _cache.GetAsync<User>(cacheKey);

        if (user is null)
        {
            user = await _context.Users
                .FirstOrDefaultAsync(
                    user => user.Id == id,
                    cancellationToken);

            if (user is null)
                throw new NotFoundException("User");
        }

        if (request.Name is not null)
            user.Name = request.Name;

        if (request.Email is not null &&
            request.Email != user.Email)
        {
            var emailExists = await _context.Users
                .AnyAsync(
                    u => u.Email == request.Email && u.Id != id,
                    cancellationToken);

            if (emailExists)
                throw new ConflictException("E-mail already exists");

            user.Email = request.Email;
        }

        if (request.Password is not null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(
                request.Password,
                10);
        }

        if (request.Phone is not null)
            user.Phone = request.Phone;
        
        // TODO: Check if user is admin to update the role
        
        //
        await _context.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(cacheKey);

        return UpdateUserResponse.FromEntity(user);
    }
}
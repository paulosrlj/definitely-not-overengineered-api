using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Features.Users.Delete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Tests.Users;

public class DeleteUserHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldRemoveUserAndInvalidateCache()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "hash",
            Role = UserRole.Customer
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set($"user:{user.Id}", user, TimeSpan.FromMinutes(1));
        var sut = new DeleteUserHandler(context, cache);

        await sut.Handle(user.Id, CancellationToken.None);

        Assert.Empty(context.Users);
        Assert.False(cache.TryGetValue($"user:{user.Id}", out _));
    }
}

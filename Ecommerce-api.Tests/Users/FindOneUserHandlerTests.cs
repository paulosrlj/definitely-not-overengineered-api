using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Features.Users.FindOne;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Tests.Users;

public class FindOneUserHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedUser_OnSecondCall()
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
        var sut = new FindOneUserHandler(context, cache);

        var first = await sut.Handle(user.Id, CancellationToken.None);
        var second = await sut.Handle(user.Id, CancellationToken.None);

        Assert.Equal(first.Email, second.Email);
        Assert.True(cache.TryGetValue($"user:{user.Id}", out _));
    }
}

using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Features.Users.Update;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Tests.Users;

public class UpdateUserHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldUpdateUserAndInvalidateCache()
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
        var sut = new UpdateUserHandler(context, cache);

        var request = new UpdateUserRequest(
            Name: "João Updated",
            Email: "joao.updated@a.com",
            Password: "new-password",
            Role: null,
            Phone: "83999999999");

        var result = await sut.Handle(user.Id, request, CancellationToken.None);

        Assert.Equal("João Updated", result.Name);
        Assert.Equal("joao.updated@a.com", result.Email);
        Assert.False(cache.TryGetValue($"user:{user.Id}", out _));
    }
}

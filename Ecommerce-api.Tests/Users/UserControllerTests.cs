using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Features.Users.Create;
using Ecommerce_api.Features.Users.Delete;
using Ecommerce_api.Features.Users.FindOne;
using Ecommerce_api.Features.Users.Update;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Tests.Users;

public class UserControllerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateUserEndpoint_ShouldReturnCreatedAtAction()
    {
        await using var context = CreateContext();
        var endpoint = new CreateUserEndpoint(new CreateUserHandler(context));

        var request = new CreateUserRequest
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "senha1234",
            Role = UserRole.Customer,
            Phone = "83999999999"
        };

        var result = await endpoint.Handle(request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateUserResponse>(created.Value);
        Assert.Equal("joao@a.com", response.Email);
    }

    [Fact]
    public async Task FindOneUserEndpoint_ShouldReturnOkResult()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "hashed",
            Role = UserRole.Customer
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var endpoint = new FindOneUserEndpoint(new FindOneUserHandler(context, new MemoryCache(new MemoryCacheOptions())));

        var result = await endpoint.Handle(user.Id, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<FindOneUserResponse>(ok.Value);
        Assert.Equal(user.Email, response.Email);
    }

    [Fact]
    public async Task UpdateUserEndpoint_ShouldReturnCreatedAtAction()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "hashed",
            Role = UserRole.Customer
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var endpoint = new UpdateUserEndpoint(new UpdateUserHandler(context, new MemoryCache(new MemoryCacheOptions())));
        var request = new UpdateUserRequest(
            Name: "João Updated",
            Email: "joao.updated@a.com",
            Password: "newpassword",
            Role: null,
            Phone: "83999999999");

        var result = await endpoint.Handle(user.Id, request, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<UpdateUserResponse>(created.Value);
        Assert.Equal("João Updated", response.Name);
        Assert.Equal("joao.updated@a.com", response.Email);
    }

    [Fact]
    public async Task DeleteUserEndpoint_ShouldReturnNoContent()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "hashed",
            Role = UserRole.Customer
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var endpoint = new DeleteUserEndpoint(new DeleteUserHandler(context, new MemoryCache(new MemoryCacheOptions())));

        var result = await endpoint.Handle(user.Id, CancellationToken.None);

        Assert.IsType<NoContentResult>(result.Result);
        Assert.Empty(context.Users);
    }
}

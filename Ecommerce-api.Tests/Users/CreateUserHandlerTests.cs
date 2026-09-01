using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Features.Users.Create;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Tests.Users;

public class CreateUserHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Handle_ShouldPersistUser_WhenEmailDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new CreateUserHandler(context);

        var request = new CreateUserRequest
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "senha1234",
            Role = UserRole.Customer,
            Phone = "83999999999"
        };

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.Equal("joao@a.com", result.Email);
        var saved = await context.Users.SingleAsync();
        Assert.Equal("João", saved.Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenEmailAlreadyExists()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "hashed",
            Role = UserRole.Customer
        });
        await context.SaveChangesAsync();

        var sut = new CreateUserHandler(context);
        var request = new CreateUserRequest
        {
            Name = "João",
            Email = "joao@a.com",
            Password = "senha1234",
            Role = UserRole.Customer,
            Phone = "83999999999"
        };

        await Assert.ThrowsAsync<ConflictException>(() => sut.Handle(request, CancellationToken.None));
    }
}

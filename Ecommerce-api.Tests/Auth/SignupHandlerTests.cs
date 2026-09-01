using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Features.Auth.Signup;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ecommerce_api.Tests.Auth;

public class SignupHandlerTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static TokenService CreateTokenService()
    {
        var settings = new JwtSettings
        {
            Key = "super-secret-key-that-is-at-least-32-characters-long",
            Issuer = "EcommerceApi",
            Audience = "EcommerceClient",
            ExpiresInMinutes = 60
        };

        return new TokenService(Options.Create(settings));
    }

    [Fact]
    public async Task Handle_ShouldCreateUserAndReturnToken_WhenEmailIsAvailable()
    {
        await using var context = CreateContext();
        var sut = new SignupHandler(context, CreateTokenService());

        var request = new SignupRequest
        {
            Name = "Maria",
            Email = "maria@a.com",
            Password = "senha1234",
            Phone = "83988887777"
        };

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
        var saved = await context.Users.SingleAsync();
        Assert.Equal("maria@a.com", saved.Email);
        Assert.Equal(UserRole.Customer, saved.Role);
    }

    [Fact]
    public async Task Handle_ShouldThrowConflict_WhenEmailAlreadyExists()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = BCrypt.Net.BCrypt.HashPassword("senha1234", 10),
            Role = UserRole.Customer
        });
        await context.SaveChangesAsync();

        var sut = new SignupHandler(context, CreateTokenService());
        var request = new SignupRequest
        {
            Name = "Jane",
            Email = "joao@a.com",
            Password = "outro123"
        };

        await Assert.ThrowsAsync<ConflictException>(() => sut.Handle(request, CancellationToken.None));
    }
}

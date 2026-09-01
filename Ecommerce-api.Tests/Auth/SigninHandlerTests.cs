using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Features.Auth.Signin;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Ecommerce_api.Tests.Auth;

public class SigninHandlerTests
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
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
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

        var sut = new SigninHandler(context, CreateTokenService());
        var request = new SigninRequest { Email = "joao@a.com", Password = "senha1234" };

        var result = await sut.Handle(request, CancellationToken.None);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Handle_ShouldThrowBadCredentials_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new SigninHandler(context, CreateTokenService());

        var request = new SigninRequest { Email = "missing@a.com", Password = "senha1234" };

        await Assert.ThrowsAsync<BadCredentialsException>(() => sut.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowBadCredentials_WhenPasswordIsInvalid()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Name = "João",
            Email = "joao@a.com",
            Password = BCrypt.Net.BCrypt.HashPassword("correct-password", 10),
            Role = UserRole.Customer
        });
        await context.SaveChangesAsync();

        var sut = new SigninHandler(context, CreateTokenService());
        var request = new SigninRequest { Email = "joao@a.com", Password = "wrong-password" };

        await Assert.ThrowsAsync<BadCredentialsException>(() => sut.Handle(request, CancellationToken.None));
    }
}

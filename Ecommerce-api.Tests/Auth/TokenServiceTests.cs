using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce_api.Tests.Auth;

public class TokenServiceTests
{
    private readonly JwtSettings _settings;
    private readonly TokenService _sut;

    public TokenServiceTests()
    {
        _settings = new JwtSettings
        {
            Key = "super-secret-key-that-is-at-least-32-characters-long",
            Issuer = "EcommerceApi",
            Audience = "EcommerceClient",
            ExpiresInMinutes = 60
        };

        _sut = new TokenService(Options.Create(_settings));
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        var token = _sut.GenerateToken(1, "user@email.com", "Admin");

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.True(new JwtSecurityTokenHandler().CanReadToken(token));
    }

    [Fact]
    public void GenerateToken_ShouldContainUserIdClaim()
    {
        var token = _sut.GenerateToken(123, "user@email.com", "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

        Assert.NotNull(claim);
        Assert.Equal("123", claim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldContainEmailClaim()
    {
        const string email = "user@email.com";

        var token = _sut.GenerateToken(1, email, "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

        Assert.NotNull(claim);
        Assert.Equal(email, claim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldContainRoleClaim()
    {
        const string role = "Admin";

        var token = _sut.GenerateToken(1, "admin@email.com", role);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        var claim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

        Assert.NotNull(claim);
        Assert.Equal(role, claim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldContainCorrectIssuer()
    {
        var token = _sut.GenerateToken(1, "user@email.com", "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal(_settings.Issuer, jwt.Issuer);
    }

    [Fact]
    public void GenerateToken_ShouldContainCorrectAudience()
    {
        var token = _sut.GenerateToken(1, "user@email.com", "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Contains(_settings.Audience, jwt.Audiences);
    }

    [Fact]
    public void GenerateToken_ShouldHaveExpirationConfigured()
    {
        var before = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes);
        var token = _sut.GenerateToken(1, "user@email.com", "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var after = DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes);

        Assert.InRange(jwt.ValidTo, before.AddSeconds(-2), after.AddSeconds(2));
    }

    [Fact]
    public void GenerateToken_ShouldUseHmacSha256Algorithm()
    {
        var token = _sut.GenerateToken(1, "user@email.com", "User");
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        Assert.Equal(SecurityAlgorithms.HmacSha256, jwt.Header.Alg);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateDifferentTokensForDifferentUsers()
    {
        var token1 = _sut.GenerateToken(1, "user1@email.com", "User");
        var token2 = _sut.GenerateToken(2, "user2@email.com", "User");

        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GenerateToken_ShouldGenerateTokenWithValidSignature()
    {
        var token = _sut.GenerateToken(1, "user@email.com", "User");

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
            ValidateIssuer = true,
            ValidIssuer = _settings.Issuer,
            ValidateAudience = true,
            ValidAudience = _settings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

        Assert.NotNull(principal);
    }
}

using System.Security.Claims;

namespace Ecommerce_api.Infrastructure.Auth;

public interface ITokenService
{
    string GenerateToken(int userId, string email, string role);
    ClaimsPrincipal? GetPrincipalFromToken(string token);

}
namespace Ecommerce_api.Infrastructure.Auth;

public interface ITokenService
{
    string GenerateToken(int userId, string email, string role);
}
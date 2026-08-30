namespace Ecommerce_api.Auth.Interfaces;

public interface ITokenService
{
    string GenerateToken(int userId, string email, string role);
}
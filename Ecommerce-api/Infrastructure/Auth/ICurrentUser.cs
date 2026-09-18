namespace Ecommerce_api.Infrastructure.Auth;

public interface ICurrentUser
{
    int Id { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
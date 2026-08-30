using Ecommerce_api.Models;

namespace Ecommerce_api.Dtos;

public record CreateUserRequest(string Name, string Email, string Password, UserRole Role, string? Phone);
public record UpdateUserRequest(string? Name = null, string? Email = null, string? Password  = null, UserRole? Role = null, string? Phone = null);

public record UserResponse(int Id, string Name, string Email, string? Phone, UserRole Role)
{
    public static UserResponse FromEntity(User user)
    {
        return new UserResponse(user.Id, user.Name, user.Email, user.Phone, user.Role);
    }
}
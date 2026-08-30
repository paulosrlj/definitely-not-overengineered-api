using Ecommerce_api.Models;

namespace Ecommerce_api.Dtos.Outbound;

public record UserResponse(int Id, string Name, string Email, string? Phone, UserRole Role)
{
    public static UserResponse FromEntity(User user)
    {
        return new UserResponse(user.Id, user.Name, user.Email, user.Phone, user.Role);
    }
}
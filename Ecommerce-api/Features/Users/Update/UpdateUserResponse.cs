using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Users.Update;

public record UpdateUserResponse(int Id, string Name, string Email, string? Phone, UserRole Role)
{
    public static UpdateUserResponse FromEntity(User user)
    {
        return new UpdateUserResponse(user.Id, user.Name, user.Email, user.Phone, user.Role);
    }
}
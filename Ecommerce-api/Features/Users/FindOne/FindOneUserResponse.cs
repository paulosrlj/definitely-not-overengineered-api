using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Users.FindOne;

public record FindOneUserResponse(int Id, string Name, string Email, string? Phone, UserRole Role)
{
    public static FindOneUserResponse FromEntity(User user)
    {
        return new FindOneUserResponse(user.Id, user.Name, user.Email, user.Phone, user.Role);
    }
}
using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Users.FindAll;

public record FindAllUsersResponse(int Id, string Name, string Email, string? Phone, UserRole Role)
{
    public static FindAllUsersResponse FromEntity(User user)
    {
        return new FindAllUsersResponse(user.Id, user.Name, user.Email, user.Phone, user.Role);
    }
}
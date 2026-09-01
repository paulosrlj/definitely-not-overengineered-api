using UserRole = Ecommerce_api.Domain.UserRole;

namespace Ecommerce_api.Features.Users.Create;

public record CreateUserResponse(
    int Id,
    string Name,
    string Email,
    string? Phone,
    UserRole Role)
{
    public static CreateUserResponse FromEntity(Domain.User user)
    {
        return new(
            user.Id,
            user.Name,
            user.Email,
            user.Phone,
            user.Role);
    }
}

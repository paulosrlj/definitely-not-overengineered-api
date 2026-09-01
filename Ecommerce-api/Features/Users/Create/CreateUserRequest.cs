using System.ComponentModel.DataAnnotations;
using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Users.Create;

public class CreateUserRequest
{
    [Required, MinLength(3), MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(3), MaxLength(50)]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    public string? Phone { get; set; }
}
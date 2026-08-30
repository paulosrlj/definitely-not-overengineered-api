using System.ComponentModel.DataAnnotations;
using Ecommerce_api.Dtos.Inbound;
using Ecommerce_api.Models;

namespace Ecommerce_api.Auth.Dtos;

public class SignupRequest
{
    [Required, MinLength(3), MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public static CreateUserModelData ToUserModelData(SignupRequest request)
    {
        return new CreateUserModelData(
            Name: request.Name,
            Email: request.Email,
            Password: request.Password,
            Phone: request.PhoneNumber,
            Role: UserRole.Customer
        );
    }
}

public class LoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}

public record LoginResponse(string Token);
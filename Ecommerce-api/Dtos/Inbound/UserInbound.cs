using System.ComponentModel.DataAnnotations;
using Ecommerce_api.Models;

namespace Ecommerce_api.Dtos.Inbound;

public class CreateUserRequest
{
    [Required, MinLength(3), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(50)]
    public string Password { get; set; }  = string.Empty;
    
    public UserRole Role { get; set; } = UserRole.Customer;
    
    public string? Phone { get; set; }
}

public record UpdateUserRequest(string? Name = null, string? Email = null, string? Password  = null, UserRole? Role = null, string? Phone = null);

public record CreateUserModelData(string Name, string Email, string Password, UserRole Role, string? Phone)
{
    public static User ToModel(CreateUserModelData data)
    {
        return new User
        {
            Name = data.Name,
            Email = data.Email,
            Password = data.Password,
            Phone = data.Phone,
            Role = data.Role
        };
    }
}




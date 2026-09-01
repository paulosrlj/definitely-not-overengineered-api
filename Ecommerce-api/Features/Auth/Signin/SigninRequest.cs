using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Features.Auth.Signin;

public class SigninRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}
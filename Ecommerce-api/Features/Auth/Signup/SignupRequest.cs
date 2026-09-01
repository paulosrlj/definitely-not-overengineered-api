using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Features.Auth.Signup;

public class SignupRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
}
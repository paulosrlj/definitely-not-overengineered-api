namespace Ecommerce_api.Domain;

public class User : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
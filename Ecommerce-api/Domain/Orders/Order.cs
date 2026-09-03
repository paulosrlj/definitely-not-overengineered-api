namespace Ecommerce_api.Domain.Orders;

public class Order : IAuditable
{
    public int Id { get; set; }

    public int Total { get; set; }

    public Address Address { get; set; } = new();

    public Status Status { get; set; } = Status.Pending;

    public string? StripeSessionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}




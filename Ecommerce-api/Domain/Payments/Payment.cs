using Ecommerce_api.Domain.Orders;

namespace Ecommerce_api.Domain.Payments;

public class Payment : IAuditable
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public PaymentProvider Provider { get; set; }
    public PaymentStatus Status { get; set; }
    
    public string? ExternalId { get; set; } = string.Empty;
    
    // The total at the time of payment processing
    public int Total { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
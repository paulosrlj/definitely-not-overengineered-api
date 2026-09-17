using Ecommerce_api.Domain.Products;

namespace Ecommerce_api.Domain.Orders;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    public int Quantity { get; set; }

    // Price at purchase moment
    public int UnitPrice { get; set; }
}


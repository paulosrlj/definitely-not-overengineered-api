using System.ComponentModel.DataAnnotations;
using Ecommerce_api.Domain.Orders;

namespace Ecommerce_api.Features.Orders.Create;

public class OrderAddress
{
    [Required, MinLength(3), MaxLength(150)]
    public string Street { get; set; } = string.Empty;
    
    [Required, Range(1, Int32.MaxValue)]
    public int Number { get; set; }
    
    [MinLength(3), MaxLength(150)]
    public string? Apartment { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(100)]
    public string City { get; set; } = string.Empty;
    
    [Required, MinLength(1), MaxLength(100)]
    public string State { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(50)]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(50)]
    public string Country { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(50)]
    public string Phone { get; set; } = string.Empty;
    
    [Required, MinLength(3), MaxLength(100)]
    public string Email { get; set; } = string.Empty;
}

public class CreateOrderItemRequest
{
    [Required, Range(1, Int32.MaxValue)]
    public int ProductId { get; set; }
    
    [Required, Range(1, Int32.MaxValue)]
    public int Quantity { get; set; }
}

public class CreateOrderRequest
{
    [Required, MinLength(1)]
    public CreateOrderItemRequest[] Items { get; set; }
    
    public OrderAddress Address  { get; set; } = new();
    
}
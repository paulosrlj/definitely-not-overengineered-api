namespace Ecommerce_api.Domain.Orders;

public enum Status {
    Pending,
    Paid,
    Shipping,
    Shipped,
    Delivered,
    Canceled,
}
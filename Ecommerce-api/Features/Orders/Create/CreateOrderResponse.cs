using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;

namespace Ecommerce_api.Features.Orders.Create;

public record CreateOrderItemResponse(
    int ProductId,
    int Quantity,
    int UnitPrice
);

public record PaymentResponse(
    int Id,
    PaymentProvider Provider,
    PaymentStatus Status,
    int Total,
    string? ExternalId
);

public record CreateOrderResponse(
    int Id,
    int Total,
    Address Address,
    Status Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    PaymentResponse Payment,
    IReadOnlyCollection<CreateOrderItemResponse> Items,
    int UserId,
    string CheckoutUrl
)
{
    public static CreateOrderResponse ToResponse(
        Order order,
        Payment payment,
        string checkoutUrl
    )
    {
        return new CreateOrderResponse(
            order.Id,
            order.Total,
            order.Address,
            order.Status,
            order.CreatedAt,
            order.UpdatedAt,
            new PaymentResponse(
                payment.Id,
                payment.Provider,
                payment.Status,
                payment.Total,
                payment.ExternalId
            ),
            order.Items
                .Select(item => new CreateOrderItemResponse(
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice
                ))
                .ToList(),
            order.UserId,
            checkoutUrl
        );
    }
};
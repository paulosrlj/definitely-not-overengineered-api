using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;

namespace Ecommerce_api.Features.Orders.FindOne;

public record FindOneOrderItemResponse(
    int ProductId,
    int Quantity,
    int UnitPrice
);

public record FindOneOrderPaymentResponse(
    int Id,
    PaymentProvider Provider,
    PaymentStatus Status,
    int Total,
    string? ExternalId
);

public record FindOneOrderResponse(
    int Id,
    int Total,
    Address Address,
    Status Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyCollection<FindOneOrderPaymentResponse> Payments,
    IReadOnlyCollection<FindOneOrderItemResponse> Items,
    int UserId
)
{
    public static FindOneOrderResponse ToResponse(
        Order order
    )
    {
        var response = new FindOneOrderResponse(
            order.Id,
            order.Total,
            order.Address,
            order.Status,
            order.CreatedAt,
            order.UpdatedAt,
            order.Payments
                .Select(payment => new FindOneOrderPaymentResponse(
                    payment.Id,
                    payment.Provider,
                    payment.Status,
                    payment.Total,
                    payment.ExternalId
                )).ToList(),
            order.Items
                .Select(item => new FindOneOrderItemResponse(
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice
                ))
                .ToList(),
            order.UserId
        );

        return response;
    }
};
using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;

namespace Ecommerce_api.Features.Orders.FindAll;

public record FindAllOrdersItemResponse(
    int ProductId,
    int Quantity,
    int UnitPrice
);

public record FindAllOrdersPaymentResponse(
    int Id,
    PaymentProvider Provider,
    PaymentStatus Status,
    int Total,
    string? ExternalId
);

public record FindAllOrdersResponse(
    int Id,
    int Total,
    Address Address,
    Status Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyCollection<FindAllOrdersPaymentResponse> Payments,
    IReadOnlyCollection<FindAllOrdersItemResponse> Items,
    int UserId
)
{
    public static IList<FindAllOrdersResponse> ToResponse(
        ICollection<Order> orders
    )
    {
        var response = orders.Select(order => new FindAllOrdersResponse(
            order.Id,
            order.Total,
            order.Address,
            order.Status,
            order.CreatedAt,
            order.UpdatedAt,
            order.Payments
                .Select(payment => new FindAllOrdersPaymentResponse(
                    payment.Id,
                    payment.Provider,
                    payment.Status,
                    payment.Total,
                    payment.ExternalId
                )).ToList(),
            order.Items
                .Select(item => new FindAllOrdersItemResponse(
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice
                ))
                .ToList(),
            order.UserId
        )).ToList();

        return response;
    }
};
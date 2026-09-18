namespace Ecommerce_api.Infrastructure.Payments;

public record PaymentItem(
    string Name,
    int Quantity,
    long UnitPrice
);

public record PaymentSessionRequest(
    int OrderId,
    string CustomerEmail,
    IReadOnlyCollection<PaymentItem> Items
);

public record PaymentSessionResult(
    string SessionId,
    string CheckoutUrl
);

public interface IPaymentService
{
    Task<PaymentSessionResult> CreateCheckoutSessionAsync(
        PaymentSessionRequest request,
        CancellationToken cancellationToken
    );
}
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Ecommerce_api.Infrastructure.Payments.Stripe;

public class StripePaymentService : IPaymentService
{
    private readonly StripeClient _client;
    private readonly StripeSettings _settings;

    public StripePaymentService(IOptions<StripeSettings> settings)
    {
        _settings = settings.Value;
        _client = new StripeClient(_settings.SecretKey);
    }

    public async Task<PaymentSessionResult> CreateCheckoutSessionAsync(PaymentSessionRequest request,
        CancellationToken cancellationToken)
    {
        var lineItems = request.Items
            .Select(item => new SessionLineItemOptions
            {
                Quantity = item.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "brl",
                    UnitAmount = item.UnitPrice,
                    ProductData =
                        new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name
                        }
                }
            })
            .ToList();
        
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            ClientReferenceId = request.OrderId.ToString(),
            CustomerEmail = request.CustomerEmail,
            LineItems = lineItems,
            SuccessUrl = "...",
            CancelUrl = "...",
            Metadata = new Dictionary<string, string>
            {
                ["order_id"] = request.OrderId.ToString()
            }
        };

        var service = _client.V1.Checkout.Sessions;
        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return new PaymentSessionResult(session.Id, session.Url);
    }
}
namespace Ecommerce_api.Infrastructure.Payments.Stripe;

public class StripeSettings
{
    public string SecretKey { get; set; }
    public string WebhookSecret { get; set; }
}
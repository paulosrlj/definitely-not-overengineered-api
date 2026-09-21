using Ecommerce_api.Features.Payments.StripeWebhook;

namespace Ecommerce_api.Features.Payments;

public static class PaymentsServiceExtension
{
    public static IServiceCollection AddPaymentFeature(
        this IServiceCollection services)
    {
        services.AddScoped<StripeWebhookHandler>();

        return services;
    }
}
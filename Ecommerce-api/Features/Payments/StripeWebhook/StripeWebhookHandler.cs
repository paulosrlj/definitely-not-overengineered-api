using Ecommerce_api.Data;
using Ecommerce_api.Domain.Orders;
using Ecommerce_api.Domain.Payments;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Payments.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Ecommerce_api.Features.Payments.StripeWebhook;

public class StripeWebhookHandler
{
    private readonly AppDbContext _context;
    private readonly StripeSettings _settings;
    private readonly ILogger<StripeWebhookHandler> _logger;

    public StripeWebhookHandler(
        AppDbContext context,
        IOptions<StripeSettings> settings,
        ILogger<StripeWebhookHandler> logger)
    {
        _context = context;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(
        HttpRequest request,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(request.Body);
        var json = await reader.ReadToEndAsync(cancellationToken);

        var signature = request.Headers["Stripe-Signature"].ToString();

        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                signature,
                _settings.WebhookSecret
            );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Invalid Stripe webhook signature"
            );

            throw new BadRequestException(
                "Invalid Stripe webhook signature"
            );
        }

        _logger.LogInformation(
            "Stripe webhook received: {EventType} {EventId}",
            stripeEvent.Type,
            stripeEvent.Id
        );

        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                await HandleCheckoutCompleted(
                    stripeEvent,
                    cancellationToken
                );
                break;

            default:
                _logger.LogInformation(
                    "Unhandled Stripe event: {EventType}",
                    stripeEvent.Type
                );
                break;
        }
    }

    private async Task HandleCheckoutCompleted(
        Event stripeEvent,
        CancellationToken cancellationToken)
    {
        if (stripeEvent.Data.Object is not Session session)
        {
            _logger.LogWarning(
                "Stripe event {EventId} does not contain a Checkout Session",
                stripeEvent.Id
            );

            return;
        }

        if (session.PaymentStatus != "paid")
        {
            _logger.LogInformation(
                "Checkout session {SessionId} completed but payment is not paid",
                session.Id
            );

            return;
        }

        var payment = await _context.Payments
            .Include(payment => payment.Order)
            .FirstOrDefaultAsync(
                payment => payment.ExternalId == session.Id,
                cancellationToken
            );

        if (payment is null)
        {
            _logger.LogWarning(
                "Payment not found for Stripe session {SessionId}",
                session.Id
            );

            return;
        }

        if (payment.Status == PaymentStatus.Paid)
        {
            _logger.LogInformation(
                "Payment {PaymentId} already marked as paid",
                payment.Id
            );

            return;
        }

        payment.Status = PaymentStatus.Paid;
        payment.Order.Status = Status.Paid;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Payment {PaymentId} and Order {OrderId} marked as paid",
            payment.Id,
            payment.OrderId
        );
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Payments.StripeWebhook;

[ApiController]
[Route("api/webhooks/stripe")]
[Tags("Webhook")]
[AllowAnonymous]
public class StripeWebhookEndpoint : ControllerBase
{
    private readonly StripeWebhookHandler _handler;

    public StripeWebhookEndpoint(StripeWebhookHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Handle(
        CancellationToken cancellationToken)
    {
        await _handler.Handle(
            Request,
            cancellationToken
        );

        return Ok();
    }
    
    // Test ENDPOINT
    [HttpGet("success")]
    public async Task<IActionResult> Success(
        CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    // Test ENDPOINT
    [HttpGet("failure")]
    public async Task<IActionResult> Fail(
        CancellationToken cancellationToken)
    {
        return BadRequest();
    }
}
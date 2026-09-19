using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Orders.Create;

[ApiController]
[Route("api/orders")]
public class CreateOrderEnpoint : ControllerBase
{
    private readonly CreateOrderHandler _handler;

    public CreateOrderEnpoint(CreateOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> Handle(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);

        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result
        );
    }
}
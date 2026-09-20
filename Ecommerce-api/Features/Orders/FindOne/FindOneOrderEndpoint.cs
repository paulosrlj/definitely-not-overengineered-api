using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Orders.FindOne;

[ApiController]
[Route("api/orders")]
[Tags("Orders")]
public class FindOneOrderEndpoint : ControllerBase
{
    private readonly FindOneOrderHandler _handler;

    public FindOneOrderEndpoint(FindOneOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FindOneOrderHandler>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(id, cancellationToken);
    
        return Ok(result);
    }
}
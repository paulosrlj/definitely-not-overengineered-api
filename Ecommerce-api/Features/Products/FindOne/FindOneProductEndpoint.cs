using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.FindOne;

[ApiController]
[Route("api/products")]
public class FindOneProductEndpoint : ControllerBase
{
    private readonly FindOneProductHandler _handler;

    public FindOneProductEndpoint(FindOneProductHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FindOneProductResponse>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
            id,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
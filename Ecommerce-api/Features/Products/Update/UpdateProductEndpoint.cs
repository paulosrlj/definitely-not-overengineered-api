using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.Update;

[ApiController]
[Route("api/products")]
[Tags("Products")]
public class UpdateProductEndpoint : ControllerBase
{
    private readonly UpdateProductHandler _handler;

    public UpdateProductEndpoint(UpdateProductHandler handler)
    {
        _handler = handler;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UpdateProductResponse>> Handle(
        int id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
            id,
            request,
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
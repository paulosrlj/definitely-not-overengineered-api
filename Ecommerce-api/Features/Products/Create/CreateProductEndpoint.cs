using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.Create;

[ApiController]
[Route("api/products")]
[Tags("Products")]
public class CreateProductEnpoint : ControllerBase
{
    private readonly CreateProductHandler _handler;

    public CreateProductEnpoint(CreateProductHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponse>> Handle(
        [FromForm] CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);
    
        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result);
    }
}
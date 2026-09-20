using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.Delete;

[ApiController]
[Route("api/products")]
[Tags("Products")]
public class DeleteProductEndpoint : ControllerBase
{
    private readonly DeleteProductHandler _handler;

    public DeleteProductEndpoint(DeleteProductHandler handler)
    {
        _handler = handler;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _handler.Handle(
            id,
            cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
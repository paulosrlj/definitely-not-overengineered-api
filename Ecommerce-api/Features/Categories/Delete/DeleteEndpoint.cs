using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.Delete;

[ApiController]
[Route("api/categories")]
public class DeleteCategoryEndpoint : ControllerBase
{
    private readonly DeleteCategoryHandler _handler;

    public DeleteCategoryEndpoint(DeleteCategoryHandler handler)
    {
        _handler = handler;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<DeleteCategoryHandler>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        await _handler.Handle(id, cancellationToken);

        return NoContent();
    }
}
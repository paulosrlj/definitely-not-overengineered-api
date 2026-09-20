using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.Update;

[ApiController]
[Route("api/categories")]
[Tags("Categories")]
public class UpdateCategoryEndpoint : ControllerBase
{
    private readonly UpdateCategoryHandler _handler;

    public UpdateCategoryEndpoint(UpdateCategoryHandler handler)
    {
        _handler = handler;
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UpdateCategoryResponse>> Handle(
        int id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken) 
    {
        var result = await _handler.Handle(id, request, cancellationToken);
    
        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result);
    }
}
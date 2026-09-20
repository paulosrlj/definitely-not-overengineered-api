using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.Create;

[ApiController]
[Route("api/categories")]
[Tags("Categories")]
public class CreateCategoryEndpoint : ControllerBase
{
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryEndpoint(CreateCategoryHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> Handle(
        [FromForm] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);
    
        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result);
    }
}
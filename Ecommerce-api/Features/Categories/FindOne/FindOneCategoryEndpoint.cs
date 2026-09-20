using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.FindOne;

[ApiController]
[Route("api/categories")]
[Tags("Categories")]
public class FindOneCategoryEndpoint : ControllerBase
{
    private readonly FindOneCategoryHandler _handler;

    public FindOneCategoryEndpoint(FindOneCategoryHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FindOneCategoryHandler>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(id, cancellationToken);
    
        return Ok(result);
    }
}
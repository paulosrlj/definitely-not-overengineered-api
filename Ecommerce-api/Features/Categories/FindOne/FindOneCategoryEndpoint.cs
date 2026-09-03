using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.FindOne;

[ApiController]
[Route("api/categories")]
public class FindOneUserEndpoint : ControllerBase
{
    private readonly FindOneCategoryHandler _handler;

    public FindOneUserEndpoint(FindOneCategoryHandler handler)
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
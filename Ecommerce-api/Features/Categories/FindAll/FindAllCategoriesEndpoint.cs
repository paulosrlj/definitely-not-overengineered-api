using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Categories.FindAll;

[ApiController]
[Route("api/categories")]
[Tags("Categories")]
public class FindAllCategoriesEndpoint : ControllerBase
{
    private readonly FindAllCategoriesHandler _handler;

    public FindAllCategoriesEndpoint(FindAllCategoriesHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<FindAllCategoriesHandler>> Handle(
        [FromQuery] FindAllCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);
    
        return Ok(result);
    }
}
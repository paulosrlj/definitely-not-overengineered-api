using Ecommerce_api.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.FindAll;

[ApiController]
[Route("api/products")]
public class FindAllProductsEndpoint : ControllerBase
{
    private readonly FindAllProductsHandler _handler;

    public FindAllProductsEndpoint(FindAllProductsHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaginatedResponse<FindAllProductsResponse>>>> Handle(
        [FromQuery] FindAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);

        return Ok(result);
    }
}
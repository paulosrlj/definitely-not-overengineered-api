using Ecommerce_api.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Products.Search;

[ApiController]
[Route("api/products")]
public class SearchProductsEndpoint : ControllerBase
{
    private readonly SearchProductsHandler _handler;

    public SearchProductsEndpoint(SearchProductsHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("/search")]
    public async Task<ActionResult<List<PaginatedResponse<SearchProductsResponse>>>> Handle(
        [FromQuery] SearchProductsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);

        return Ok(result);
    }
}
using Ecommerce_api.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Orders.FindAll;

[ApiController]
[Route("api/orders")]
[Tags("Orders")]
public class FindAllOrdersEndpoint : ControllerBase
{
    private readonly FindAllOrdersHandler _handler;

    public FindAllOrdersEndpoint(FindAllOrdersHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaginatedResponse<FindAllOrdersResponse>>>> Handle(
        [FromQuery] FindAllOrdersRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);

        return Ok(result);
    }
}
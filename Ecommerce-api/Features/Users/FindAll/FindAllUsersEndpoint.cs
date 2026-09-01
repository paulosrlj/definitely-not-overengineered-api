using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Users.FindAll;

[ApiController]
[Route("api/users")]
public class FindAllUsersEndpoint : ControllerBase
{
    private readonly FindAllUsersHandler _handler;

    public FindAllUsersEndpoint(FindAllUsersHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<ActionResult<FindAllUsersHandler>> Handle(
        [FromQuery] FindAllUsersRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);
    
        return Ok(result);
    }
}
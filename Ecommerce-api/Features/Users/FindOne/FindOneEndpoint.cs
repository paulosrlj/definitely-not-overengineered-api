using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Users.FindOne;

[ApiController]
[Route("api/users")]
[Tags("Users")]
public class FindOneUserEndpoint : ControllerBase
{
    private readonly FindOneUserHandler _handler;

    public FindOneUserEndpoint(FindOneUserHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FindOneUserHandler>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(id, cancellationToken);
    
        return Ok(result);
    }
}
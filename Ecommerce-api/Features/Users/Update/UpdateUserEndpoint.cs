using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Users.Update;

[ApiController]
[Route("api/users")]
[Tags("Users")]
public class UpdateUserEndpoint : ControllerBase
{
    private readonly UpdateUserHandler _handler;

    public UpdateUserEndpoint(UpdateUserHandler handler)
    {
        _handler = handler;
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UpdateUserResponse>> Handle(
        int id,
        UpdateUserRequest request,
        CancellationToken cancellationToken) 
    {
        var result = await _handler.Handle(id, request, cancellationToken);
    
        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result);
    }
}
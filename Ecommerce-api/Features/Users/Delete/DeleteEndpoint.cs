using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Users.Delete;

[ApiController]
[Route("api/users")]
public class DeleteUserEndpoint : ControllerBase
{
    private readonly DeleteUserHandler _handler;

    public DeleteUserEndpoint(DeleteUserHandler handler)
    {
        _handler = handler;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<DeleteUserHandler>> Handle(
        int id,
        CancellationToken cancellationToken)
    {
        await _handler.Handle(id, cancellationToken);

        return NoContent();
    }
}
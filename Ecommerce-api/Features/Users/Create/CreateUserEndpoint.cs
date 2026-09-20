using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Users.Create;

[ApiController]
[Route("api/users")]
[Tags("Users")]
public class CreateUserEndpoint : ControllerBase
{
    private readonly CreateUserHandler _handler;

    public CreateUserEndpoint(CreateUserHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Handle(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(request, cancellationToken);
    
        return CreatedAtAction(
            nameof(Handle),
            new { id = result.Id },
            result);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Auth.Signup;

[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class SignupEndpoint : ControllerBase
{
    private readonly SignupHandler _handler;

    public SignupEndpoint(SignupHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult<SignupResponse>> Handle(
        SignupRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _handler.Handle(
            request,
            cancellationToken);

        return Ok(response);
    }
}
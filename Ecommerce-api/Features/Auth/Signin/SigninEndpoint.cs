using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_api.Features.Auth.Signin;

[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class SigninEndpoint : ControllerBase
{
    private readonly SigninHandler _handler;

    public SigninEndpoint(SigninHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<SigninResponse>> Handle(
        SigninRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _handler.Handle(
            request,
            cancellationToken);

        return Ok(response);
    }
}
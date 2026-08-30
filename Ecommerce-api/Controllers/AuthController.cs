using Ecommerce_api.Auth.Dtos;
using Ecommerce_api.Auth.Interfaces;
using Ecommerce_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Ecommerce_api.Auth.Dtos.LoginRequest;

namespace Ecommerce_api.controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService,  IUsersService usersService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        => Ok(await _authService.LoginAsync(request));

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Register(SignupRequest request)
    {
        var token = await _authService.SignupAsync(request);
        return Ok(token);
    }
}
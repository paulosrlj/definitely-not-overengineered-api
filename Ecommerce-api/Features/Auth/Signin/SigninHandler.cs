using Ecommerce_api.Data;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Auth.Signin;

public class SigninHandler
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<SigninHandler> _logger;

    public SigninHandler(AppDbContext context, ITokenService tokenService,  ILogger<SigninHandler> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }
    
    public async Task<SigninResponse> Handle(
        SigninRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken);

        if (user is null)
        {
            _logger.LogWarning(
                "Authentication failed");
            throw new BadCredentialsException("Invalid credentials");
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.Password);

        if (!validPassword)
        {
            _logger.LogWarning(
                "Authentication failed for user {UserId}",
                user.Id);
            throw new BadCredentialsException("Invalid credentials");
        }

        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role.ToString());
        
        _logger.LogInformation(
            "User {UserId} authenticated successfully",
            user.Id);


        return new SigninResponse(token);
    }
}
using Ecommerce_api.Data;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Auth.Signin;

public class SigninHandler
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    
    public SigninHandler(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
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
            throw new BadCredentialsException("Invalid credentials");

        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.Password);

        if (!validPassword)
            throw new BadCredentialsException("Invalid credentials");

        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role.ToString());

        return new SigninResponse(token);
    }
}
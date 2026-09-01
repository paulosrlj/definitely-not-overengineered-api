using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Auth.Signup;

public class SignupHandler
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;
    
    public SignupHandler(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }
    
    public async Task<SignupResponse> Handle(
        SignupRequest request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email == request.Email,
                cancellationToken);

        if (existingUser is not null)
            throw new ConflictException("User already exists");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(
            request.Password,
            10);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = hashedPassword,
            Phone = request.Phone,
            Role = UserRole.Customer
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync(cancellationToken);

        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role.ToString());

        return new SignupResponse(token);
    }
}
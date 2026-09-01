using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Users.Create;

public class CreateUserHandler
{
    private readonly AppDbContext _context;
    
    public CreateUserHandler(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var exists = await _context.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);
    
        if (exists)
            throw new ConflictException("E-mail already exists!");
    
        var user = new User()
        {
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, 10),
            Role = request.Role,
            Phone = request.Phone
        };
    
        _context.Users.Add(user);
    
        await _context.SaveChangesAsync(cancellationToken);
    
        return CreateUserResponse.FromEntity(user);
    }
}
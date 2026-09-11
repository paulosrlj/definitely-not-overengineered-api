using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Users.Create;

public class CreateUserHandler
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(AppDbContext dbContext, ILogger<CreateUserHandler> logger)
    {
        _context = dbContext;
        _logger = logger;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new user");
        var exists = await _context.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (exists)
        {
            _logger.LogWarning(
                "User creation rejected because the email already exists");


            throw new ConflictException("E-mail already exists!");
        }

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

        _logger.LogInformation(
            "User {UserId} created successfully",
            user.Id
        );

        return CreateUserResponse.FromEntity(user);
    }
}
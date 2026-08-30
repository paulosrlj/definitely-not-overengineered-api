using Ecommerce_api.Dtos;
using Ecommerce_api.Models;

namespace Ecommerce_api.Repositories.Interfaces;

public record CreateUserData(string Name, string Email, string Password, UserRole Role, string? Phone)
{
    public static User ToModel(CreateUserData data)
    {
        return new User
        {
            Name = data.Name,
            Email = data.Email,
            Password = data.Password,
            Phone = data.Phone,
            Role = data.Role
        };
    }
}

public record UpdateUserData(string? Name, string? Email, string? Password, string? Phone);

public interface IUserRepository
{
    Task<User> CreateAsync(CreateUserData data);
    Task<User?> FindByIdAsync(int id);
    Task<User?> FindByEmailAsync(string email);
    Task<(List<User> Users, int Total)> FindManyAsync(string? search, int skip, int take);
    Task<User?> UpdateAsync(int id, UpdateUserData data);
    Task<bool> DeleteAsync(int id);
}
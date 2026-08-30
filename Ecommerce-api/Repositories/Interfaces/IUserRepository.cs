using Ecommerce_api.Dtos;
using Ecommerce_api.Dtos.Inbound;
using Ecommerce_api.Models;

namespace Ecommerce_api.Repositories.Interfaces;


public record UpdateUserData(string? Name, string? Email, string? Password, string? Phone);

public interface IUserRepository
{
    Task<User> CreateAsync(CreateUserModelData data);
    Task<User?> FindByIdAsync(int id);
    Task<User?> FindByEmailAsync(string email);
    Task<(List<User> Users, int Total)> FindManyAsync(string? search, int skip, int take);
    Task<User?> UpdateAsync(int id, UpdateUserData data);
    Task<bool> DeleteAsync(int id);
}
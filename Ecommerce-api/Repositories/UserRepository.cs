using Ecommerce_api.Data;
using Ecommerce_api.Dtos.Inbound;
using Ecommerce_api.Models;
using Ecommerce_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User> CreateAsync(CreateUserModelData data)
    {
        var user = CreateUserModelData.ToModel(data);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<User?> FindByIdAsync(int id) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> FindByEmailAsync(string email) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<(List<User> Users, int Total)> FindManyAsync(string? search, int skip, int take)
    {
        var query = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{search}%") ||
                EF.Functions.ILike(u.Email, $"%{search}%"));
        }

        var total = await query.CountAsync();
        var users = await query.Skip(skip).Take(take).ToListAsync();
        return (users, total);
    }

    public async Task<User?> UpdateAsync(int id, UpdateUserData data)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return null;

        if (data.Name is not null) user.Name = data.Name;
        if (data.Email is not null) user.Email = data.Email;
        if (data.Password is not null) user.Password = data.Password;
        if (data.Phone is not null) user.Phone = data.Phone;

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
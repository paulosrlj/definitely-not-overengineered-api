using Ecommerce_api.Dtos;
using Ecommerce_api.Dtos.Inbound;
using Ecommerce_api.Dtos.Outbound;
using Ecommerce_api.Dtos.Request;
using Ecommerce_api.Dtos.Response;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Models;
using Ecommerce_api.Repositories.Interfaces;
using Ecommerce_api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce_api.Services;

public class UserService : IUsersService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _cache;

    public UserService(IUserRepository userRepository, IMemoryCache cache)
    {
        _userRepository = userRepository;
        _cache = cache;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var existing = await _userRepository.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new ConflictException("E-mail already exists!");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);

        var user = await _userRepository.CreateAsync(
            new CreateUserModelData(request.Name, request.Email, hashedPassword, request.Role, request.Phone));

        return UserResponse.FromEntity(user);
    }

    public async Task<PaginatedResponse<UserResponse>> FindAllAsync(FindEntitiesQuery query)
    {
        var skip = (query.Page - 1) * query.Limit;
        var (users, total) = await _userRepository.FindManyAsync(query.Search, skip, query.Limit);

        return new PaginatedResponse<UserResponse>(
            users.Select(UserResponse.FromEntity).ToList(), total, query.Page, query.Limit);
    }

    public async Task<UserResponse> FindOneAsync(int id)
    {
        var cacheKey = $"user:{id}";

        if (_cache.TryGetValue<User>(cacheKey, out var cachedUser) && cachedUser is not null)
            return UserResponse.FromEntity(cachedUser);

        var user = await _userRepository.FindByIdAsync(id)
            ?? throw new NotFoundException("User");

        _cache.Set(cacheKey, user, TimeSpan.FromMinutes(1));
        return UserResponse.FromEntity(user);
    }

    public async Task<UserResponse> FindByEmailAsync(string email)
    {
        var user = await  _userRepository.FindByEmailAsync(email) ?? throw new NotFoundException("User");
        
        return  UserResponse.FromEntity(user);
    }

    public async Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request)
    {
        var hashedPassword = request.Password is not null
            ? BCrypt.Net.BCrypt.HashPassword(request.Password)
            : null;

        var user = await _userRepository.UpdateAsync(id,
            new UpdateUserData(request.Name, request.Email, hashedPassword, request.Phone))
            ?? throw new NotFoundException("User");

        _cache.Remove($"user:{id}");
        return UserResponse.FromEntity(user);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        if (!deleted) throw new NotFoundException("User");

        _cache.Remove($"user:{id}");
        return true;
    }
}
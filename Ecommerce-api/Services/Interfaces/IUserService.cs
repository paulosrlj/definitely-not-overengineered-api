using Ecommerce_api.Dtos.Inbound;
using Ecommerce_api.Dtos.Outbound;
using Ecommerce_api.Dtos.Request;
using Ecommerce_api.Dtos.Response;

namespace Ecommerce_api.Services.Interfaces;

public interface IUsersService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request);
    Task<PaginatedResponse<UserResponse>> FindAllAsync(FindEntitiesQuery query);
    Task<UserResponse> FindOneAsync(int id);
    Task<UserResponse> FindByEmailAsync(string email);
    Task<UserResponse> UpdateAsync(int id, UpdateUserRequest request);
    Task<bool> RemoveAsync(int id);
}
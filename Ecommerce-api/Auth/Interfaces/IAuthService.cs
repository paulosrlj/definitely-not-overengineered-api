using Ecommerce_api.Auth.Dtos;
using Ecommerce_api.Dtos.Inbound;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Ecommerce_api.Auth.Dtos.LoginRequest;

namespace Ecommerce_api.Auth.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> SignupAsync(SignupRequest request);
}
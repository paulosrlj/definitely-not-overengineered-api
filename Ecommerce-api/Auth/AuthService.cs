using Ecommerce_api.Auth.Dtos;
using Ecommerce_api.Auth.Interfaces;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Repositories.Interfaces;
using LoginRequest = Ecommerce_api.Auth.Dtos.LoginRequest;

namespace Ecommerce_api.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        Console.WriteLine("Chegou aqui");
        var user = await _userRepository.FindByEmailAsync(request.Email)
                   ?? throw new BadCredentialsException("Invalid credentials");
        
        Console.WriteLine(request.Password);
        Console.WriteLine(user.Password);

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!isValid)
            throw new BadCredentialsException("Invalid credentials");

        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        return new LoginResponse(token);
    }

    public async Task<LoginResponse> SignupAsync(SignupRequest request)
    {
        var existingUser = await _userRepository.FindByEmailAsync(request.Email);
        if (existingUser is not null) throw new ConflictException("User already exists");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, 10);
        request.Password = hashedPassword;
        
        var user = await _userRepository.CreateAsync(SignupRequest.ToUserModelData(request));
        
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        
        return new LoginResponse(token);
    }
}
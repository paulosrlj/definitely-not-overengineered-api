using Ecommerce_api.Dtos;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Models;
using Ecommerce_api.Repositories.Interfaces;
using Ecommerce_api.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Ecommerce_api.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _sut = new UserService(_repository.Object, _cache);
    }

    [Fact]
    public async Task MustCreateUserWhenDoesNotExists()
    {
        _repository.Setup(r => r.FindByEmailAsync("joao@a.com")).ReturnsAsync((User?)null);
        _repository.Setup(r => r.CreateAsync(It.IsAny<CreateUserData>()))
            .ReturnsAsync(new User { Id = 1, Name = "João", Email = "joao@a.com" });

        var result = await _sut.CreateAsync(
            new CreateUserRequest(
                Name: "João",
                Email: "joao@a.com",
                Password: "senha1234",
                Role: UserRole.Customer,
                Phone: ""
            ));
        
        Assert.Equal("joao@a.com", result.Email);
        _repository.Verify(r => r.CreateAsync(It.IsAny<CreateUserData>()), Times.Once);
    }

    [Fact]
    public async Task MustThrowConflictExceptionIfEmailAlreadyExists()
    {
        _repository.Setup(r => r.FindByEmailAsync("joao@a.com"))
            .ReturnsAsync(new User { Id = 1, Email = "joao@a.com" });

        await Assert.ThrowsAsync<ConflictException>(() =>
            _sut.CreateAsync(new CreateUserRequest(
                Name: "João",
                Email: "joao@a.com",
                Password: "senha1234",
                Role: UserRole.Customer,
                Phone: ""
            )));

        _repository.Verify(r => r.CreateAsync(It.IsAny<CreateUserData>()), Times.Never);
    }

    [Fact]
    public async Task MustReturnUserWhenFound()
    {
        _repository.Setup(r => r.FindByIdAsync(1))
            .ReturnsAsync(new User { Id = 1, Name = "João", Email = "joao@a.com" });

        var result = await _sut.FindOneAsync(1);

        Assert.Equal(1, result.Id);
        _repository.Verify(r => r.FindByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task MustThrowNotFoundExceptionIfNotFound()
    {
        _repository.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((User?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.FindOneAsync(999));
    }

    [Fact]
    public async Task MustUseCacheWhenInSecondCall()
    {
        _repository.Setup(r => r.FindByIdAsync(1))
            .ReturnsAsync(new User { Id = 1, Name = "João", Email = "joao@a.com" });

        await _sut.FindOneAsync(1); // popula o cache
        await _sut.FindOneAsync(1); // deve vir do cache

        _repository.Verify(r => r.FindByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task MustThrowNotFoundExceptionifNotFound()
    {
        _repository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.RemoveAsync(999));
    }
}
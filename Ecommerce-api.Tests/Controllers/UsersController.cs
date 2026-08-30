using Ecommerce_api.controllers;
using Ecommerce_api.Dtos;
using Ecommerce_api.Models;
using Ecommerce_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Ecommerce_api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUsersService> _service = new();
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _controller = new UsersController(_service.Object);
    }

    [Fact]
    public async Task MustCreateUserWithCorrectParams()
    {
        var request = new CreateUserRequest(
            Name: "João",
            Email: "joao@a.com",
            Password: "senha1234",
            Role: UserRole.Customer,
            Phone: ""
        );

        var response = new UserResponse(1, "João", "joao@a.com", null, UserRole.Customer);
        _service.Setup(s => s.CreateAsync(request)).ReturnsAsync(response);

        var result = await _controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(response, created.Value);
    }

    [Fact]
    public async Task MustFindUserWithId()
    {
        var response = new UserResponse(1, "João", "joao@a.com", null, UserRole.Customer);
        _service.Setup(s => s.FindOneAsync(1)).ReturnsAsync(response);

        var result = await _controller.FindOne(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, ok.Value);
    }

    [Fact]
    public async Task MustReturnUserResponseAfterUpdate()
    {
        var request = new UpdateUserRequest(
            Name: "João updated",
            Email: "joao_updated@a.com",
            Phone: "83999999999",
            Role: UserRole.Customer
        );

        var responseUpdated = new UserResponse(
            1,
            "João updated",
            "joao_updated@a.com",
            "83999999999",
            UserRole.Customer
        );

        _service
            .Setup(s => s.UpdateAsync(1, request))
            .ReturnsAsync(responseUpdated);

        var result = await _controller.Update(1, request);

        var ok = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, ok.StatusCode);
        Assert.Equal(responseUpdated, ok.Value);
    }


    [Fact]
    public async Task MustReturnNoContentAfterUserDelete()
    {
        _service.Setup(s => s.RemoveAsync(1)).ReturnsAsync(true);

        var result = await _controller.Remove(1);

        Assert.IsType<NoContentResult>(result);
        _service.Verify(s => s.RemoveAsync(1), Times.Once);
    }
}
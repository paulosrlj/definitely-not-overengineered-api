using Ecommerce_api.Dtos;
using Ecommerce_api.Dtos.Request;
using Ecommerce_api.Dtos.Response;
using Ecommerce_api.Models;
using Ecommerce_api.Services;
using Ecommerce_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    public UsersController(IUsersService usersService) => _usersService = usersService;

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
    {
        var result = await _usersService.CreateAsync(request);
        return CreatedAtAction(nameof(FindOne), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<UserResponse>>> FindAll([FromQuery] FindEntitiesQuery query)
        => Ok(await _usersService.FindAllAsync(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> FindOne(int id)
        => Ok(await _usersService.FindOneAsync(id));

    [HttpPatch("{id}")]
    public async Task<ActionResult<UserResponse>> Update(int id, UpdateUserRequest request)
        => Ok(await _usersService.UpdateAsync(id, request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        await _usersService.RemoveAsync(id);
        return NoContent();
    }
}
using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Controllers.v1;

public class UserController : ApiControllerBase
{
    private readonly IRepository<User> _repository;
    
    public UserController(IRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpGet("context")]
    public IActionResult GetContext()
    {
        return Ok(User.Claims);
    }
    
    [Authorize(Roles = "api:admin")]
    [HttpGet("")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _repository.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _repository.GetQueryable()
            .Include(i => i.DocumentsOwnered)
            .Include(i => i.DocumentsAuthorized)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(user);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateUser(UserDto user)
    {
        var entity = new User
        {
            Name = user.Name,
            Function = user.Function
        };
        var createdUser = await _repository.AddAsync(entity);
        return Ok(createdUser);
    }
    
    [HttpPut("{id} ")]
    public async Task<IActionResult> UpdateUser(int id, UserDto user)
    {
        var entity = new User
        {
            Id = id,
            Name = user.Name,
            Function = user.Function
        };
        var updatedUser = await _repository.UpdateAsync(entity);
        return Ok(updatedUser);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deletedUser = await _repository.DeleteAsync(id);
        return Ok(deletedUser);
    }
}

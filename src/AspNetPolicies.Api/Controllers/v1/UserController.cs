using AspNetPolicies.Data.Repositories;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AspNetPolicies.Api.Controllers.v1;

[ApiVersion("1.0")]
public class UserController : ApiControllerBase
{
    private readonly IRepository<User> _userRepository;
    
    public UserController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return Ok(user);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateUser(User user)
    {
        var createdUser = await _userRepository.AddAsync(user);
        return Ok(createdUser);
    }
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateUser(User user)
    {
        var updatedUser = await _userRepository.UpdateAsync(user);
        return Ok(updatedUser);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deletedUser = await _userRepository.DeleteAsync(id);
        return Ok(deletedUser);
    }
}

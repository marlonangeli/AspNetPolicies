using AspNetPolicies.Api.Constants;
using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace AspNetPolicies.Api.Controllers.v1;

public class UserController : ApiControllerBase
{
    private readonly IRepository<User> _repository;
    private readonly IAuthorizationService _authorizationService;

    public UserController(IRepository<User> repository, IAuthorizationService authorizationService)
    {
        _repository = repository;
        _authorizationService = authorizationService;
    }

    [Authorize(Roles = Roles.READ)]
    [HttpGet("context")]
    public IActionResult GetContext()
    {
        return Ok(User.Claims);
    }

    [Authorize(Roles = Roles.READ)]
    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = new object[]
        {
            new { Name = "Read", Value = User.HasClaim(ClaimTypes.Role, Roles.READ) },
            new { Name = "Write", Value = User.HasClaim(ClaimTypes.Role, Roles.WRITE) },
            new { Name = "Admin", Value = User.HasClaim(ClaimTypes.Role, Roles.ADMIN) },
        };

        var document = User.Claims.FirstOrDefault(x => x.Type == "document");
        if (document != null)
        {
            var json = JsonConvert.DeserializeObject(document.Value);
            permissions = permissions.Append(new { Name = "Document", Value = json }).ToArray();
        }

        // test policy from code
        var policy = new AuthorizationPolicyBuilder();
        policy.RequireClaim(ClaimTypes.Role, Roles.READ);
        var authResult = await _authorizationService.AuthorizeAsync(User, permissions, Policies.USER_OWNER_DOCUMENT);
        if (authResult.Succeeded)
        {
            permissions = permissions.Append(new { Name = "Policy", Value = "UserOwnerDocument" }).ToArray();
        }
        
        return Ok(permissions);
    }

    [Authorize(Roles = Roles.ADMIN)]
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

    [Authorize(Roles = $"{Roles.ADMIN},{Roles.WRITE}")]
    [HttpPut("{id}")]
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

    [Authorize(Roles = Roles.ADMIN)]
    [Authorize(Roles = Roles.DELETE)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deletedUser = await _repository.DeleteAsync(id);
        return Ok(deletedUser);
    }
}
using AspNetPolicies.Api.Constants;
using AspNetPolicies.Security.Helpers;
using AspNetPolicies.Security.Models;
using Newtonsoft.Json;

namespace AspNetPolicies.Api.Security.Permissions;

public class UserPermission : Permission
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserPermission(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        Name = "User";
        Description = "User permission";
    }

    public override void SetActions()
    {
        var actions = _httpContextAccessor.HttpContext.User.GetClaimValue(ClaimTypes.USER);
        if (actions != null)
        {
            Actions = JsonConvert.DeserializeObject<Actions>(actions);
        }
    }
}
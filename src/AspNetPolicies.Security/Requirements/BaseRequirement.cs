using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace AspNetPolicies.Security.Requirements;

public abstract class BaseRequirement : IAuthorizationRequirement
{
    public string Policy { get; set; }

    public BaseRequirement(string policy)
    {
        Policy = policy;
    }
    
    public abstract bool IsAuthorized(AuthorizationHandlerContext context);
}
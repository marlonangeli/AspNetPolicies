using Microsoft.AspNetCore.Authorization;

namespace AspNetPolicies.Security.Requirements;

public abstract class BaseRequirement : IAuthorizationRequirement
{
    private string Policy { get; }

    public BaseRequirement(string policy)
    {
        Policy = policy;
    }
    
    public abstract bool IsAuthorized(AuthorizationHandlerContext context);
}
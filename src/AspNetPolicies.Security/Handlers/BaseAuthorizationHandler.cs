using AspNetPolicies.Security.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace AspNetPolicies.Security.Handlers;

public abstract class BaseAuthorizationHandler<T> : AuthorizationHandler<T> where T : BaseRequirement
{
    public abstract Task<bool> IsAuthorized(AuthorizationHandlerContext context, T requirement);

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        if (await IsAuthorized(context, requirement))
            context.Succeed(requirement);
    }
}
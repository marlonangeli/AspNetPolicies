using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetPolicies.Security.Extensions;

public static class PolicyExtension 
{
    public static void AddPolicy<T>(this IServiceCollection services, string policyName, T requirement)
        where T : IAuthorizationRequirement
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.Requirements.Add(requirement);
            });
        });
    }
    
    public static void AddPolicy<T>(this IServiceCollection services, string policyName, params T[] requirements)
        where T : IAuthorizationRequirement
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                foreach (var requirement in requirements)
                {
                    policy.Requirements.Add(requirement);
                }
            });
        });
    }
}
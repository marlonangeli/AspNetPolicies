using AspNetPolicies.Security.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AspNetPolicies.Security.PolicyProviders;

public abstract class BasePolicyProvider<TRequirement> : IAuthorizationPolicyProvider where TRequirement : BaseRequirement, new()
{
    public const string PolicyPrefix = "Policy_";
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    protected BasePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    
    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        try
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new TRequirement { Policy = PolicyPrefix + policyName });
            return Task.FromResult(policy.Build());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        FallbackPolicyProvider.GetFallbackPolicyAsync();
}
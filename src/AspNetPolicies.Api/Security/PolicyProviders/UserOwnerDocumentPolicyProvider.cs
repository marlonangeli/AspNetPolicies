using AspNetPolicies.Api.Security.Requirements;
using AspNetPolicies.Security.PolicyProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AspNetPolicies.Api.Security.PolicyProviders;

public class UserOwnerDocumentPolicyProvider : BasePolicyProvider<UserOwnerDocumentRequirement>
{
    public UserOwnerDocumentPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }
}
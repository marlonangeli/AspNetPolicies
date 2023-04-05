using AspNetPolicies.Api.Security.Handlers;
using AspNetPolicies.Api.Security.PolicyProviders;
using AspNetPolicies.Api.Security.Requirements;
using AspNetPolicies.Data.Repositories;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AspNetPolicies.Api.Extensions;

public static class ServicesInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IAuthorizationRequirement, UserOwnerDocumentRequirement>();
        services.AddTransient<IAuthorizationHandler, UserOwnerDocumentHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, UserOwnerDocumentPolicyProvider>();
    }
}
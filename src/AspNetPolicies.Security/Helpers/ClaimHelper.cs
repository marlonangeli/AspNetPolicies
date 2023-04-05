using System.Security.Claims;

namespace AspNetPolicies.Security.Helpers;

public static class ClaimHelper
{
    public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
    {
        return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    }
    
    public static string GetClaimValue(this ClaimsPrincipal user, string claimType, string defaultValue)
    {
        return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? defaultValue;
    }
}
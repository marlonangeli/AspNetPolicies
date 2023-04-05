using AspNetPolicies.Api.Constants;
using AspNetPolicies.Security.Helpers;
using AspNetPolicies.Security.Models;
using Newtonsoft.Json;

namespace AspNetPolicies.Api.Security.Permissions;

public class DocumentPermission : Permission
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DocumentPermission(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        Name = "Document";
        Description = "Document permission";
    }

    public override void SetActions()
    {
        var actions = _httpContextAccessor.HttpContext.User.GetClaimValue(ClaimTypes.DOCUMENT);
        if (actions != null)
        {
            Actions = JsonConvert.DeserializeObject<Actions>(actions);
        }
    }
}
using AspNetPolicies.Api.Constants;
using AspNetPolicies.Security.Requirements;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace AspNetPolicies.Api.Security.Requirements;

public class UserOwnerDocumentRequirement : BaseRequirement
{
    public int UserId { get; set; }

    public UserOwnerDocumentRequirement() : base(Policies.USER_OWNER_DOCUMENT)
    {
    }

    public override bool IsAuthorized(AuthorizationHandlerContext context)
    {
        var json = context.User.Claims.Where(x => x.Type == "sicop_attributes").Select(x => x.Value).FirstOrDefault();
        if (string.IsNullOrEmpty(json))
            return false;
        var userId = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        if (!userId.ContainsKey("userId"))
            return false;
        UserId = userId["userId"];
        return true;
    }
}
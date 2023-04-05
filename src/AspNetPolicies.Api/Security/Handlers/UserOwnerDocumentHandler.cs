using AspNetPolicies.Api.Security.Requirements;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using AspNetPolicies.Security.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Security.Handlers;

public class UserOwnerDocumentHandler : BaseAuthorizationHandler<UserOwnerDocumentRequirement>
{
    private readonly IRepository<Document> _documentRepository;

    public UserOwnerDocumentHandler(IRepository<Document> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public override async Task<bool> IsAuthorized(AuthorizationHandlerContext context, UserOwnerDocumentRequirement requirement)
    {
        if (!requirement.IsAuthorized(context))
            return false;

        var doc = await _documentRepository.GetQueryable()
            .FirstOrDefaultAsync(x => x.OwnerUserId == requirement.UserId);

        return doc != null;
    }
}
using AspNetPolicies.Api.Security.Requirements;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using AspNetPolicies.Security.Exceptions;
using AspNetPolicies.Security.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Security.Handlers;

public class UserOwnerDocumentHandler : BaseAuthorizationHandler<UserOwnerDocumentRequirement>
{
    private readonly IRepository<Document> _documentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserOwnerDocumentHandler(IRepository<Document> documentRepository, IHttpContextAccessor httpContextAccessor)
    {
        _documentRepository = documentRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<bool> IsAuthorized(AuthorizationHandlerContext context, UserOwnerDocumentRequirement requirement)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            throw new UnauthorizedException("User is not authenticated");
        }
        
        if (!requirement.IsAuthorized(context))
            return false;

        var documentId = int.Parse(_httpContextAccessor.HttpContext.Request.RouteValues["id"].ToString());
        
        var doc = await _documentRepository.GetQueryable()
            .AsNoTracking()
            .Where(x => x.OwnerUserId == requirement.UserId && x.Id == documentId)
            .FirstOrDefaultAsync();

        if (doc == null)
            throw new UnauthorizedException("User does not have permission to access this document");
        
        return true;
    }
}
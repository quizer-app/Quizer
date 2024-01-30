using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Quizer.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly Permission _permission;

    public HasPermissionAttribute(Permission permission)
    {
        _permission = permission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // TODO: delete this ?

        // Check if the user is authenticated
        if (context.HttpContext.User.Identity is null || !context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check if the user has the required claim
        var hasClaim = context.HttpContext.User.HasClaim(CustomClaims.Permission, _permission.ToString());
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}

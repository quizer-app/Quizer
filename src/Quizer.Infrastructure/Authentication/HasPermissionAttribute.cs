using Microsoft.AspNetCore.Authorization;

namespace Quizer.Infrastructure.Authentication;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: PermissionAuthorizationPolicy.For(permission))
    {
    }
}

using Microsoft.AspNetCore.Authorization;

namespace Quizer.Infrastructure.Authentication;

public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission permission)
    {
        Permission = permission;
    }

    public Permission Permission { get; }
}

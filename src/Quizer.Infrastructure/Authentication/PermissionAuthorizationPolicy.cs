namespace Quizer.Infrastructure.Authentication;

public static class PermissionAuthorizationPolicy
{
    private const string Prefix = "permission:";

    public static string For(Permission permission) => $"{Prefix}{permission}";
}

namespace Bss.Infrastructure;

public static class Modules
{
    public static Type[] InfrastructureModules =
    [
        typeof(Configuration.Module),
        typeof(Components.Module),
        typeof(Identity.Module),
        typeof(WebHost.Module),
    ];
}

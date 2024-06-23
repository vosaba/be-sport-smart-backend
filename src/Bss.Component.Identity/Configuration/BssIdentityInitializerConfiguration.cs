using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Component.Identity.Configuration;

[Configuration]
public class BssIdentityInitializerConfiguration
{
    public bool CreateSuperAdmin { get; set; }
    
    public string SuperAdminRole { get; set; } = string.Empty;

    public string SuperAdminEmail { get; set; } = string.Empty;

    public string SuperAdminUserName { get; set; } = string.Empty;

    public string SuperAdminPassword { get; set; } = string.Empty;
}
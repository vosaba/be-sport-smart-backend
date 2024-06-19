using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Infrastructure.WebHost.Configurations;

[Configuration]
internal class SecurityConfiguration
{
    public string[] AllowedOrigins { get; set; } = [];
}

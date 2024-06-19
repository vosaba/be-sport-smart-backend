using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Infrastructure.Components.Configurations;

[Flags]
public enum SecurityDefinition
{
    BearerJwt = 1,
    ApiKey = 2,
    Basic = 4,
}

[Configuration]
internal class ComponentsConfiguration
{
    public SecurityDefinitionConfiguration SecurityDefinition { get; set; } = new();

    public class SecurityDefinitionConfiguration
    {
        public SecurityDefinition EnabledSecurityDefinitions { get; set; }

        public string JwtTokenProviderUrl { get; set; } = string.Empty;
    }
}

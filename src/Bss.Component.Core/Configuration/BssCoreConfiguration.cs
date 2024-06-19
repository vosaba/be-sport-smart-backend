using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Component.Core.Configuration;

[Configuration("BssCore")]
public class BssCoreConfiguration
{
    public string Test { get; set; } = string.Empty;
}

[Configuration("BssCore2")]
public class BssCore2Configuration
{
    public string Test { get; set; } = string.Empty;
}

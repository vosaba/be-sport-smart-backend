using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Dal.Configuration;

[Configuration]
public class BssDalConfiguration
{
    public required string PostgresVersion { get; set; } = string.Empty;
    public required ConnectionStringsConfiguration ConnectionStrings { get; set; }

    public class ConnectionStringsConfiguration
    {
        public string BssCore { get; set; } = string.Empty;
        public string BssIdentity { get; set; } = string.Empty;
    }
}

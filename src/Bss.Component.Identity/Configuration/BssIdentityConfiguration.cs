using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Component.Identity.Configuration;

[Configuration]
public class BssIdentityConfiguration
{
    public JwtConfiguration Jwt { get; set; } = new();

    public class JwtConfiguration
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SigningKey { get; set; } = string.Empty;
        public int ExpireHours { get; set; }
    }
}

using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Identity.Configuration;

[Configuration]
public class BssIdentityConfiguration
{
    public string RedirectUrl { get; set; } = string.Empty;
    public JwtConfiguration Jwt { get; set; } = new();
    public OAuthConfiguration Github { get; set; } = new();

    public class JwtConfiguration
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SigningKey { get; set; } = string.Empty;
        public int ExpireHours { get; set; }
    }

    public class OAuthConfiguration
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}

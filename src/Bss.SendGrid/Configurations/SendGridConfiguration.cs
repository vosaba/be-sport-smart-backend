using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.SendGrid.Configurations;

[Configuration]
public class SendGridConfiguration
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
}

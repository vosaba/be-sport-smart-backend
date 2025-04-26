using Bss.Core.Bl.Services;
using Bss.SendGrid.Configurations;
using Bss.SendGrid.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

namespace Bss.SendGrid;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSendGrid((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IOptions<SendGridConfiguration>>();

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            options.ApiKey = configuration.Value.ApiKey;
        });

        services.AddScoped<INotificationService, NotificationService>();
    }
}

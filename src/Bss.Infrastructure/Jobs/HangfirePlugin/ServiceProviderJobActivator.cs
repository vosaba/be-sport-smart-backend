using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Jobs.HangfirePlugin;

internal class ServiceProviderJobActivator(IServiceProvider serviceProvider) : JobActivator
{
    public override object ActivateJob(Type jobType)
    {
        return serviceProvider.GetRequiredService(jobType);
    }
}

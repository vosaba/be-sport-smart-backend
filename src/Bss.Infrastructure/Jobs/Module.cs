using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Jobs.Configurations;
using Bss.Infrastructure.Jobs.HangfirePlugin;
using Bss.Infrastructure.Jobs.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bss.Infrastructure.Jobs;

internal class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ServiceProviderJobActivator>();
        services.AddScoped<IJobRunner, JobRunner>();

        services.AddHangfire((serviceProvider, config) => {
            config.UseInMemoryStorage();
            config.UseActivator(serviceProvider.GetRequiredService<ServiceProviderJobActivator>());
        });
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 1;
        });
    }

    public void Configure(IApplicationBuilder app, IOptions<JobsConfiguration> jobsConfiguration, IRecurringJobManager recurringJobs)
    {
        var configuration = jobsConfiguration.Value;

        if (configuration.Dashboard.Enabled)
        {
            app.UseHangfireDashboard(configuration.Dashboard.Path, new DashboardOptions
            {
                Authorization = new[]
                {
                    new RoleDashboardAuthorizationFilter()
                },
                IsReadOnlyFunc = context => configuration.Dashboard.ReadOnly
            });
        }

        recurringJobs.Configure(configuration);
    }
}
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Jobs.Configurations;
using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bss.Infrastructure.Jobs.HangfirePlugin;

internal class RoleDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext dashboardContext)
    {
        var serviceProvider = dashboardContext.GetHttpContext().RequestServices;
        var userContext = serviceProvider.GetService<IUserContext>();
        var configuration = serviceProvider.GetService<IOptions<JobsConfiguration>>();

        if (userContext == null || configuration == null)
        {
            return false;
        }

        return configuration.Value.Dashboard.Roles.Any(userContext.IsInRole);
    }
}
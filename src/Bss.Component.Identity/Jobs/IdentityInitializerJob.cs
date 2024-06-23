using Bss.Component.Identity.Configuration;
using Bss.Component.Identity.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Jobs.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bss.Component.Identity.Jobs;

[Job(nameof(IdentityInitializerJob))]
public class IdentityInitializerJob(
    ILogger<IdentityInitializerJob> logger,
    IOptions<BssIdentityInitializerConfiguration> options,
    UserManager<ApplicationUser> userManager)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var config = options.Value;

        if (!config.CreateSuperAdmin)
        {
            logger.LogInformation("Super admin creation is disabled.");
            return;
        }

        logger.LogInformation("Creating super admin...");

        var existingAdmin = await userManager.FindByNameAsync(config.SuperAdminUserName);

        if (existingAdmin != null)
        {
            logger.LogInformation("Super admin already exists.");
            return;
        }

        var admin = new ApplicationUser
        {
            UserName = config.SuperAdminUserName,
            Email = config.SuperAdminRole,
        };

        var result = await userManager.CreateAsync(admin, config.SuperAdminPassword);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description), OperationErrorCodes.InvalidRequest);
        }

        result = await userManager.AddToRoleAsync(admin, config.SuperAdminRole);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description));
        }

        logger.LogInformation("Super admin created.");
    }
}

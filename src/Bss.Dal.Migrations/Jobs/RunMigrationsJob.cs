using Bss.Infrastructure.Jobs.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Dal.Migrations.Jobs;

[Job(nameof(RunMigrationsJob))]
public class RunMigrationsJob(
    ILogger<RunMigrationsJob> logger,
    CoreDbContext coreDbContext,
    IdentityDbContext identityDbContext)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await MigrateAllDbs(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    private Task MigrateAllDbs(CancellationToken cancellationToken)
        => Task.WhenAll(
            coreDbContext.Database.MigrateAsync(cancellationToken),
            identityDbContext.Database.MigrateAsync(cancellationToken));
}

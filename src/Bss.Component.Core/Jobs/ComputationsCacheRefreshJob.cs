using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Jobs;

[Job(nameof(ComputationsCacheRefreshJob))]
public class ComputationsCacheRefreshJob(
    ICoreDbContext dbContext,
    ILogger<MeasuresCacheRefreshJob> logger,
    ILocalCacheCollection<Computation> computationCacheCollection)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to refresh computations cache collections.");

        var computations = await dbContext
            .Computations
            .Where(x => x.Disabled == false)
            .ToListAsync(cancellationToken);

        computationCacheCollection.Overwrite(computations);

        logger.LogTrace("Cache collections refreshed.");
    }
}
using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Jobs;

[Job(nameof(CachesInitializerJob))]
public class CachesInitializerJob(
    IJobRunner jobRunner,
    ICoreDbContext dbContext,
    ILogger<CachesInitializerJob> logger,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ILocalCacheCollection<Measure> measureCacheCollection)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to initialize cache collections.");

        var computations = await dbContext
            .Computations
            .Where(x => x.Disabled == false)
            .ToListAsync(cancellationToken);

        var measures = await dbContext
            .Measures
            .Where(x => x.Disabled == false)
            .ToListAsync(cancellationToken);

        computationCacheCollection.AddRange(computations);
        measureCacheCollection.AddRange(measures);

        logger.LogTrace("Cache collections initialized.");

        jobRunner.Trigger<ComputationEnginesInitializerJob>();
    }
}
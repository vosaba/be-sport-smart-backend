using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Jobs;

[Job(nameof(CacheInitializerJob))]
public class CacheInitializerJob(
    IJobRunner jobRunner,
    ICoreDbContext dbContext,
    ILogger<CacheInitializerJob> logger,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ILocalCacheCollection<Measure> measureCacheColelction)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to initialize cache collections.");

        var computations = await dbContext
            .Computations
            .ToListAsync(cancellationToken);

        var measures = await dbContext
            .Measures
            .ToListAsync(cancellationToken);

        computationCacheCollection.AddRange(computations);
        measureCacheColelction.AddRange(measures);

        logger.LogTrace("Cache collections initialized.");

        jobRunner.Trigger<ComputationEnginesInitializerJob>();
    }
}
using Bss.Component.Core.Data;
using Bss.Component.Core.Data.Models;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Jobs;

[Job(nameof(MeasuresCacheRefreshJob))]
public class MeasuresCacheRefreshJob(
    ICoreDbContext dbContext,
    ILogger<MeasuresCacheRefreshJob> logger,
    ILocalCacheCollection<Measure> measureCacheCollection)
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to refresh measures cache collections.");

        var measures = await dbContext
            .Measures
            .Where(x => x.Disabled == false)
            .ToListAsync(cancellationToken);

        measureCacheCollection.Overwrite(measures);

        logger.LogTrace("Cache collections refreshed.");
    }
}

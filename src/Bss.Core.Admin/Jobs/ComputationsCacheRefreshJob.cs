using Bss.Core.Bl.Data;
using Bss.Core.Bl.Events.ComputationsCacheRefreshed;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Admin.Jobs;

[Job(nameof(ComputationsCacheRefreshJob))]
public class ComputationsCacheRefreshJob(
    IMediator mediator,
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
            .AsNoTracking()
            .Where(x => x.Disabled == false)
            .ToListAsync(cancellationToken);

        computationCacheCollection.Overwrite(computations);

        await mediator.Publish(new ComputationsCacheRefreshedEvent(), cancellationToken);

        logger.LogTrace("Cache collections refreshed.");
    }
}
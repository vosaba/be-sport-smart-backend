using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Events.ComputationListChange;

internal class ComputationListChangeEventHandler(
    ICoreDbContext dbContext,
    ILogger<ComputationListChangeEventHandler> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection) 
    : INotificationHandler<ComputationListChangeEvent>
{
    public async Task Handle(ComputationListChangeEvent @event, CancellationToken cancellationToken)
    {
        logger.LogTrace("Computation list change event received.");

        var computationEngine = computationEngineFactory
            .GetService(@event.ComputationEngine);

        var computations = await dbContext
            .Computations
            .ToListAsync(cancellationToken);

        computationCacheCollection.Overwrite(computations);

        computationEngine.RefreshContext(computations.Where(x => x.Engine == @event.ComputationEngine));

        logger.LogTrace("Computation list change event handled.");
    }
}

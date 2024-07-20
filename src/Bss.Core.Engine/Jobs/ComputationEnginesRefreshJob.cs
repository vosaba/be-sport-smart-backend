using Bss.Core.Bl.Models;
using Bss.Core.Engine.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Jobs;

[Job(nameof(ComputationEnginesRefreshJob))]
public class ComputationEnginesRefreshJob(
    ILogger<ComputationEnginesRefreshJob> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection) 
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to initialize computation engines.");

        if (computationCacheCollection.IsEmpty)
        {
            throw new OperationException("Computation cache collection is empty.", OperationErrorCodes.InvalidOperation);
        } 

        var computations = computationCacheCollection.GetAll();

        foreach (var engine in computations.Select(x => x.Engine).Distinct())
        {
            try
            {
                logger.LogTrace($"Initializing computation engine {engine}.");

                var computationEngine = computationEngineFactory.GetService(engine);

                computationEngine.RefreshContext(computations.Where(x => x.Engine == engine));

                logger.LogTrace($"Computation engine {engine} initialized.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to initialize computation engine {engine}.");
            }
        }

        logger.LogTrace("Computation engines initialized.");
    }
}
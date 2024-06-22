using Bss.Component.Core.Configuration;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bss.Component.Core.Jobs;

[Job(nameof(ComputationEnginesInitializerJob))]
public class ComputationEnginesInitializerJob(
    IOptions<BssCoreConfiguration> config,
    ILogger<ComputationEnginesInitializerJob> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection) 
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to initialize computation engines.");

        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache collection is empty. Skipping initialization.");
            return;
        } 

        var computations = computationCacheCollection.GetAll();

        foreach (var engine in config.Value.SupportedEngines)
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
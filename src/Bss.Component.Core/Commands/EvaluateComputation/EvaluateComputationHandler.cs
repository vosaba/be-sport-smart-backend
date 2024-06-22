using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.EvaluateComputation;

public class EvaluateComputationHandler(
    ILogger<EvaluateComputationHandler> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection)
{
    public async Task<EvaluateComputationResponse> Handle(EvaluateComputationRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");
        }

        var availableComputations = computationCacheCollection.GetAll();

        var computation = availableComputations
            .SingleOrDefault(x => x.Name == request.Name) 
            ?? throw new NotFoundException(request.Name, nameof(Computation));

        var computationEngine = computationEngineFactory.GetService(computation.Engine);
        if (!computationEngine.ContextInitialized)
        {
            computationEngine.RefreshContext(availableComputations.Where(x => x.Engine == computation.Engine));
        }

        var result = await computationEngine.Evaluate<double>(computation);
        return new EvaluateComputationResponse
        {
            Result = result
        };
    }
}

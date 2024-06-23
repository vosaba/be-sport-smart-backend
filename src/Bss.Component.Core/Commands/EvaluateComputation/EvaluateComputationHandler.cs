using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Bss.Component.Core.Commands.EvaluateComputation;

[AllowAnonymous]
public class EvaluateComputationHandler(
    ILogger<EvaluateComputationHandler> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ILocalCacheCollection<Measure> measureCacheCollection)
{
    public async Task<EvaluateComputationResponse> Handle(EvaluateComputationRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");
        }

        if (measureCacheCollection.IsEmpty)
        {
            logger.LogWarning("Measure cache is empty.");
        }

        var availableComputations = computationCacheCollection.GetAll();

        var computation = availableComputations
            .SingleOrDefault(x => x.Name == request.Name) 
            ?? throw new NotFoundException(request.Name, nameof(Computation));

        if (computation.RequiredMeasures.Count > 0 && !computation.RequiredMeasures.Any(request.MeasureValues.ContainsKey))
        {
            throw new OperationException(
                "Missing measure values.", 
                computation.RequiredMeasures.Except(request.MeasureValues.Keys),
                OperationErrorCodes.InvalidRequest);
        }

        var computationEngine = computationEngineFactory.GetService(computation.Engine);
        if (!computationEngine.ContextInitialized)
        {
            logger.LogWarning("Computation engine context not initialized.");
        }

        var availableMeasures = measureCacheCollection
            .GetAll()
            .Where(x => computation.RequiredMeasures.Contains(x.Name))
            .ToDictionary(x => x.Name, x => x.Type);

        var measureValues = computation.RequiredMeasures.Select(x => new MeasureValue(x, availableMeasures[x], request.MeasureValues[x])).ToArray();

        var result = await computationEngine.Evaluate<double>(computation, measureValues);
        return new EvaluateComputationResponse
        {
            Result = result
        };
    }
}

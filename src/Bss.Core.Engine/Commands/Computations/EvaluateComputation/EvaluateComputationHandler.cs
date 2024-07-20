using Bss.Core.Bl.Models;
using Bss.Core.Engine.Services.ComputationEngines;
using Bss.Core.Engine.Services.ComputationRequirements;
using Bss.Core.Engine.Services.MeasureValues;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Commands.Computations.EvaluateComputation;

[AllowAnonymous]
public class EvaluateComputationHandler(
    IUserContext userContext,
    ILogger<EvaluateComputationHandler> logger,
    IMeasureValueService measureValueService,
    IComputationRequirementService computationRequirementService,
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
            .SingleOrDefault(x =>
                x.Name == request.Name
                && x.IsExecutableByUser(userContext.IsAuthenticated))
            ?? throw new NotFoundException(request.Name, nameof(Computation));

        var measureValues = measureValueService
            .GetValidMeasureValues(request.MeasureValues)
            .ToArray();

        computationRequirementService.EnsureRequiredMeasureProvided(computation, measureValues);

        var computationEngine = computationEngineFactory.GetService(computation.Engine);
        if (!computationEngine.ContextInitialized)
        {
            logger.LogWarning("Computation engine context not initialized.");
        }

        var result = await computationEngine.Evaluate<double>(computation, measureValues);
        return new EvaluateComputationResponse
        {
            Result = result
        };
    }
}

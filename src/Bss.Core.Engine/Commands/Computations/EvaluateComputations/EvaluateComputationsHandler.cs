using Bss.Core.Bl.Models;
using Bss.Core.Engine.Dto;
using Bss.Core.Engine.Services.ComputationEngines;
using Bss.Core.Engine.Services.ComputationRequirements;
using Bss.Core.Engine.Services.MeasureValues;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Commands.Computations.EvaluateComputations;

[AllowAnonymous]
public class EvaluateComputationsHandler(
    IUserContext userContext,
    ILogger<EvaluateComputationsHandler> logger,
    IMeasureValueService measureValueService,
    IComputationRequirementService computationRequirementService,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection)
{
    public async Task<ComputationResultDto[]> Handle(EvaluateComputationsRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");

            return [];
        }

        var availableComputations = computationCacheCollection
            .GetAll()
            .Where(x =>
                x.Type == request.Type
                && x.IsExecutableByUser(userContext.IsAuthenticated));

        if (request.Names.Length > 0)
        {
            availableComputations = availableComputations
                .Where(x => request.Names.Contains(x.Name));
        }

        var measureValues = measureValueService
            .GetValidMeasureValues(request.MeasureValues)
            .ToArray();

        var computationTasks = availableComputations
            .Select(async x =>
            {
                var result = await TryEvaluateComputation<double>(x, measureValues);
                return new ComputationResultDto
                {
                    Name = x.Name,
                    Result = result ?? double.NaN
                };
            });

        return [.. (await Task.WhenAll(computationTasks)).OrderBy(x => x.Result)];
    }

    private async Task<TResult?> TryEvaluateComputation<TResult>(
        Computation computation,
        MeasureValue[] measureValues)
        where TResult : struct
    {
        try
        {
            if (!computationRequirementService.IsRequiredMeasureProvided(computation, measureValues))
            {
                return null;
            }

            var computationEngine = computationEngineFactory.GetService(computation.Engine);

            return await computationEngine.Evaluate<TResult>(computation, measureValues);
        }
        catch (OperationException ex)
        {
            logger.LogWarning(ex, ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return null;
        }
    }
}

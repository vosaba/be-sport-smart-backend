using Bss.Component.Core.Dto;
using Bss.Component.Core.Enums;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.EvaluateComputations;

[AllowAnonymous]
public class EvaluateComputationsHandler(
    ILogger<EvaluateComputationsHandler> logger,
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ILocalCacheCollection<Measure> measureCacheCollection)
{
    public async Task<ComputationResultDto[]> Handle(EvaluateComputationsRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");

            return [];
        }

        if (measureCacheCollection.IsEmpty)
        {
            logger.LogWarning("Measure cache is empty.");

            return [];
        }

        var availableComputations = computationCacheCollection
            .GetAll()
            .Where(x => x.Type == request.Type);

        if (request.Names.Length > 0)
        {
            availableComputations = availableComputations
                .Where(x => request.Names.Contains(x.Name));
        }

        var availableMeasures = measureCacheCollection
            .GetAll()
            .ToDictionary(x => x.Name, x => x.Type);

        var computationTasks = availableComputations
            .Select(async x =>
            {
                var result = await TryEvaluateComputation<double>(x, availableMeasures, request.MeasureValues);
                return new ComputationResultDto
                {
                    Name = x.Name,
                    Result = result ?? double.NaN
                };
            });

        return await Task.WhenAll(computationTasks);
    }

    private async Task<TResult?> TryEvaluateComputation<TResult>(
        Computation computation,
        IDictionary<string, MeasureType> measureTypes,
        Dictionary<string, string> providedValues)
        where TResult : struct
    {
        if (computation.RequiredMeasures.Count > 0 && computation.RequiredMeasures.Any(x => !providedValues.ContainsKey(x)))
        {
            return null;
        }

        var computationEngine = computationEngineFactory.GetService(computation.Engine);

        try
        {
            var measureValues = providedValues
                .Where(x => measureTypes.ContainsKey(x.Key))
                .Select(x => new MeasureValue(x.Key, measureTypes[x.Key], x.Value))
                .ToArray();

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

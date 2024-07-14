using Bss.Component.Core.Enums;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;

namespace Bss.Component.Core.Services;

public interface IMeasureValueService
{
    public IEnumerable<MeasureValue> GetValidMeasureValues(Dictionary<string, string> measureValues);
}

internal class MeasureValueService(ILocalCacheCollection<Measure> measureCacheCollection) : IMeasureValueService
{
    public IEnumerable<MeasureValue> GetValidMeasureValues(Dictionary<string, string> measureValues)
    {
        var availableMeasures = measureCacheCollection
            .GetAll();

        foreach (var measureValue in measureValues)
        {
            var measure = availableMeasures.SingleOrDefault(x => x.Name == measureValue.Key) 
                ?? throw new NotFoundException(measureValue.Key, nameof(Measure));

            if (measure.Disabled)
            {
                throw new OperationException($"Measure '{measure.Name}' is disabled.", OperationErrorCodes.Forbidden);
            }

            EnsureMeasureValid(measure, measureValue.Value);

            yield return new MeasureValue(measure.Name, measure.Type, measureValue.Value);
        }
    }

    private static void EnsureMeasureValid(Measure measure, string value)
    {
        if (measure.Options.Length > 0 && !measure.Options.Any(x => x == value))
        {
            throw new OperationException($"Value '{value}' is not a valid option for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
        }

        switch (measure.Type)
        {
            case MeasureType.Number:
                if (!double.TryParse(value, out var numberValue))
                {
                    throw new OperationException($"Value '{value}' is not a valid number for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
                }

                if (measure.MinValue.HasValue && numberValue < measure.MinValue.Value)
                {
                    throw new OperationException($"Value '{value}' is less than the minimum value '{measure.MinValue}' for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
                }

                if (measure.MaxValue.HasValue && numberValue > measure.MaxValue.Value)
                {
                    throw new OperationException($"Value '{value}' is greater than the maximum value '{measure.MaxValue}' for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
                }

                break;
            case MeasureType.String:
                break;
            case MeasureType.Boolean:
                if (!bool.TryParse(value, out _))
                {
                    throw new OperationException($"Value '{value}' is not a valid boolean for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
                }
                break;
            default:
                throw new OperationException($"Unknown measure type '{measure.Type}' for measure '{measure.Name}'.", OperationErrorCodes.InvalidRequest);
        }
    }
}

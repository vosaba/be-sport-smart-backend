﻿using Bss.Component.Core.Enums;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Jint;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Services.ComputationEngines;

internal class JsComputationEngine(ILogger<JsComputationEngine> logger) : IComputationEngine
{
    private readonly Engine _engine = new();
    private List<Computation>? _computations;

    public bool ContextInitialized => _computations is not null;

    public IEnumerable<Computation> Context
        => _computations ?? throw new OperationException("Context not initialized.");

    public bool ContextEmpty => _computations is not null && _computations.Count > 0;

    public void ClearContext() 
    {
        if (_computations is null)
        {
            throw new OperationException("Context not initialized.");
        }

        _computations.Clear(); 
    }

    public void RefreshContext(IEnumerable<Computation> computations)
    {
        _computations ??= [];
        _engine.Execute($"internal_context = {{}};");

        foreach (var computation in computations)
        {
            var contextArea = computation.Type switch
            {
                ComputationType.Metric => "metrics",
                ComputationType.Score => "scores",
                ComputationType.Sport => "sports",
                _ => throw new OperationException($"Invalid computation type: {computation.Type}")
            };

            try
            {
                _engine.Execute($"if (typeof internal_context.{contextArea} === 'undefined') {{ internal_context.{contextArea} = {{}}; }}");
                _engine.Execute($"internal_context.{contextArea}['{computation.Name}'] = {computation.Formula};");
                _computations.Add(computation);
            }
            catch (Exception ex)
            {
                throw new OperationException($"Error initializing context for computation: {computation.Name} with type {computation.Type}", ex);
            }
        }
    }

    public async Task<TResult> Evaluate<TResult>(Computation computation, params MeasureValue[] measureValues)
        where TResult : struct
    {
        if (computation.Type == ComputationType.Score)
        {
            throw new OperationException(
                "Score computations are not supported for direct evaluation as they may require additional parameters",
                OperationErrorCodes.InvalidOperation);
        }

        try
        {
            var measuresString = GetMeasuresJson(measureValues);
            var functionString = GetComputationFunction(computation);
            var evaluationResult = _engine.Evaluate($"{functionString}({measuresString});");

            // TODO: add more flexible result handling
            return typeof(TResult) switch
            {
                Type t when t == typeof(double) => (TResult)(object)evaluationResult.AsNumber(),
                Type t when t == typeof(bool) => (TResult)(object)evaluationResult.AsBoolean(),
                Type t when t == typeof(string) => (TResult)(object)evaluationResult.AsString(),
                _ => throw new OperationException($"Unsupported result type: {typeof(TResult)}"),
            };
        }
        catch (Exception ex)
        {
            throw new OperationException("Error evaluating formula", ex);
        }
    }

    public async Task EnsureExecutable(Computation computation, params MeasureValue[] measureValues)
    {
        var existingComputation = Context
            .Where(x => x.Name == computation.Name)
            .SingleOrDefault() 
            ?? throw new NotFoundException(computation.Name, nameof(Computation));

        var missingMeasure = measureValues.FirstOrDefault(measureValue => !existingComputation.RequiredMeasures.Contains(measureValue.Name));
        if (missingMeasure != null)
        {
            throw new OperationException($"Required measure value not provided: {missingMeasure.Name}");
        }

        return;
    }

    public void Dispose() => _engine.Dispose();

    private string GetMeasuresJson(params MeasureValue[] measureValues)
    {
        string FormatValue(MeasureValue measureValue)
        {
            switch (measureValue.Type)
            {
                case MeasureType.Boolean:
                    if (bool.TryParse(measureValue.Value, out var boolResult))
                    {
                        return boolResult.ToString().ToLower();
                    }
                    else
                    {
                        var result = "false";
                        LogDefaultValueUsage(measureValue, result);
                        return result;
                    }
                case MeasureType.Number:
                    if (double.TryParse(measureValue.Value, out var numberResult))
                    {
                        return numberResult.ToString();
                    }
                    else
                    {
                        var result = "0";
                        LogDefaultValueUsage(measureValue, result);
                        return result;
                    }
                case MeasureType.String:
                    return $"\"{measureValue.Value}\"";
                default:
                    LogDefaultValueUsage(measureValue, measureValue.Value);
                    return measureValue.Value;
            }
        }

        void LogDefaultValueUsage(MeasureValue measureValue, string usedValue) 
            => logger.LogWarning("Default value set for key '{name}'. Original value: '{value}', Default value: '{usedValue}'", measureValue.Name, measureValue.Value, usedValue);

        return $"{{{string.Join(",", measureValues.Select(x => $"'{x.Name}': {FormatValue(x)}"))}}}";
    }

    private static string GetComputationFunction(Computation computation)
    {
        var contextArea = computation.Type switch
        {
            ComputationType.Metric => "metrics",
            ComputationType.Sport => "sports",
            _ => throw new OperationException($"Invalid computation type: {computation.Type} for evaluation")
        };

        return @$"(function(measures) 
        {{ 
            var context = {{ 
                measures: measures, 
                metrics: internal_context.metrics, 
                scores: internal_context.scores 
            }};
                    
            return internal_context.{contextArea}.{computation.Name}(context); 
        }})";
    }
}

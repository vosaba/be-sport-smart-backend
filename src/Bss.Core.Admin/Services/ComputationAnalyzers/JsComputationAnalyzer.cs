using Bss.Core.Bl.Data;
using Bss.Core.Bl.Models;
using Bss.Core.Bl.Services.ComputationAnalyzers;
using Bss.Infrastructure.Errors.Abstractions;
using Jint;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bss.Core.Admin.Services.ComputationAnalyzers;

public partial class JsComputationAnalyzer(ICoreDbContext dbContext) : IComputationAnalyzer
{
    private readonly Engine _testEngine = new();

    public async Task EnsureValid(Computation computation)
    {
        var computations = await dbContext.Computations
            .Where(x => computation.RequiredComputations.Contains(x.Name) && x.Engine == computation.Engine && !x.Disabled)
            .ToListAsync();

        var measures = await dbContext.Measures
            .Where(x => computation.RequiredMeasures.Contains(x.Name) && !x.Disabled)
            .ToListAsync();

        if (computations.Count != computation.RequiredComputations.Count)
        {
            throw new OperationException($"Missing required computations: {string.Join(", ", computation.RequiredComputations.Except(computations.Select(x => x.Name)))}");
        }

        if (measures.Count != computation.RequiredMeasures.Count)
        {
            throw new OperationException($"Missing required measures: {string.Join(", ", computation.RequiredMeasures.Except(measures.Select(x => x.Name)))}");
        }

        try
        {
            _testEngine.Execute($"var testComputation = {computation.Formula};");

            if (_testEngine.Evaluate("typeof testComputation === 'function'").AsBoolean() != true)
            {
                throw new ValidationException(new ValidationResult("Formula must be a JS function", [nameof(computation.Formula)]), null, null);
            }
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            throw new OperationException("Error validating formula", ex, OperationErrorCodes.InvalidRequest);
        }
    }

    public async Task<ComputationRequirements> GetComputationRequirements(Computation computation)
        => new ComputationRequirements(
            ComputationsUsed()
                .Matches(computation.Formula)
                .Select(m => m.Groups[2].Value)
                .Distinct()
                .ToList(),
            MeasuresUsed()
                .Matches(computation.Formula)
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToList());

    public void Dispose() => _testEngine.Dispose();

    [GeneratedRegex(@"context\.(metrics|scores)\.(\w+)")]
    private static partial Regex ComputationsUsed();

    [GeneratedRegex(@"context\.measures\.(\w+)")]
    private static partial Regex MeasuresUsed();
}

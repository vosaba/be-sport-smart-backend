using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Jint;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Bss.Component.Core.Services;

public interface IComputationValidator : IDisposable
{
    public Task EnsureValidComputation(Computation computation);
}

public class ComputationValidator : IComputationValidator
{
    private readonly Engine _testEngine = new();
    private readonly ICoreDbContext _dbContext;

    public ComputationValidator(ICoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task EnsureValidComputation(Computation computation)
    {
        var computations = await _dbContext.Computations
            .Where(x => computation.RequiredComputations.Contains(x.Name) && !x.Disabled)
            .ToListAsync();

        var measures = await _dbContext.Measures
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

            // TODO: Add evaluation of the testComputation with context of required computations and measures
        }
        catch (Exception ex) when (ex is not ValidationException)
        {
            throw new OperationException(ex.Message, OperationErrorCodes.InvalidRequest);
        }
    }

    public void Dispose() => _testEngine.Dispose();
}

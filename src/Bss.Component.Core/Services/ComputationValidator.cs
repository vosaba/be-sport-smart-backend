using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Jint;
using Microsoft.EntityFrameworkCore;

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
            throw new InvalidOperationException($"Missing required computations: {string.Join(", ", computation.RequiredComputations.Except(computations.Select(x => x.Name)))}");
        }

        if (measures.Count != computation.RequiredMeasures.Count)
        {
            throw new InvalidOperationException($"Missing required measures: {string.Join(", ", computation.RequiredMeasures.Except(measures.Select(x => x.Name)))}");
        }

        try
        {
            _testEngine.Execute($"var testComputation = {computation.Formula};");

            if (_testEngine.Evaluate("typeof testComputation === 'function'").AsBoolean() != true)
            {
                throw new InvalidOperationException("Formula must be a JS function");
            }

            // TODO: Add evaluation of the testComputation with context of required computations and measures
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public void Dispose() => _testEngine.Dispose();
}

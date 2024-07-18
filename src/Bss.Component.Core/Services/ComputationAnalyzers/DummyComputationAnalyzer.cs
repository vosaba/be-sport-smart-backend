using Bss.Component.Core.Data.Models;

namespace Bss.Component.Core.Services.ComputationAnalyzers;

public class DummyComputationAnalyzer : IComputationAnalyzer
{
    public void Dispose()
    {
    }

    public Task EnsureValid(Computation computation)
    {
        return Task.CompletedTask;
    }

    public Task<ComputationRequirements> GetComputationRequirements(Computation computation)
    {
        return Task.FromResult(new ComputationRequirements([], []));
    }
}
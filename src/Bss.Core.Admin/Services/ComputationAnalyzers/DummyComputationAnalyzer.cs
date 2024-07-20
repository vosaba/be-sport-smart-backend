using Bss.Core.Bl.Models;
using Bss.Core.Bl.Services.ComputationAnalyzers;

namespace Bss.Core.Admin.Services.ComputationAnalyzers;

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
using Bss.Component.Core.Models;

namespace Bss.Component.Core.Services.ComputationAnalyzers;

public interface IComputationAnalyzer : IDisposable
{
    public Task EnsureValid(Computation computation);

    public Task<ComputationRequirements> GetComputationRequirements(Computation computation);
}

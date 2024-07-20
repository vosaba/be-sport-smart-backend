
using Bss.Core.Bl.Models;
using Bss.Core.Bl.Services.ComputationAnalyzers;

namespace Bss.Core.Admin.Services.ComputationAnalyzers;

public interface IComputationAnalyzer : IDisposable
{
    public Task EnsureValid(Computation computation);

    public Task<ComputationRequirements> GetComputationRequirements(Computation computation);
}

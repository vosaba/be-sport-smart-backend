using Bss.Component.Core.Models;

namespace Bss.Component.Core.Services.ComputationEngines;

public interface IComputationEngine : IDisposable
{
    public bool ContextInitialized { get; }

    public bool ContextEmpty { get; }

    public IEnumerable<Computation> Context { get; }

    public void ClearContext();

    public Task<TResult> Evaluate<TResult>(Computation computation, params MeasureValue[] measureValues)
        where TResult : struct;

    Task EnsureExecutable(Computation computation, params MeasureValue[] measureValues);

    public void RefreshContext(IEnumerable<Computation> computations);
}
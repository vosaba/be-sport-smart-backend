using Bss.Component.Core.Enums;
using Bss.Component.Core.Data.Models;

namespace Bss.Component.Core.Services.ComputationEngines;

public class DummyComputationEngine : IComputationEngine
{
    public bool ContextInitialized => true;

    public bool ContextEmpty => true;

    public IEnumerable<Computation> Context => throw new NotImplementedException();

    public void ClearContext() { }

    public IEnumerable<Computation> GetContextComputations() => [];

    public void RefreshContext(IEnumerable<Computation> computations) { }

    public Task<TResult> Evaluate<TResult>(Computation computation, params MeasureValue[] measureValues) where TResult : struct
    {
        throw new NotImplementedException();
    }

    public Task EnsureExecutable(Computation computation, params MeasureValue[] measureValues)
    {
        throw new NotImplementedException();
    }

    public void Dispose() 
    {
    }
}
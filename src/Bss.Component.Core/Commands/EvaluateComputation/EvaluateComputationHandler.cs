using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Bss.Infrastructure.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.EvaluateComputation;

public class EvaluateComputationHandler(
    IServiceFactory<IComputationEngine> computationEngineFactory,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ICoreDbContext coreDbContext)
{
    public async Task<EvaluateComputationResponse> Handle(EvaluateComputationRequest request)
    {
        var (computations, computationsRefreshed) = await computationCacheCollection
            .InitializeOrGetListAsync(async () => await coreDbContext.Computations.ToListAsync());

        var computation = computations
            .SingleOrDefault(x => x.Name == request.Name) 
            ?? throw new NotFoundException(request.Name, nameof(Computation));

        var computationEngine = computationEngineFactory.GetService(computation.Engine);

        if (!computationEngine.ContextInitialized || computationsRefreshed)
        {
            computationEngine.RefreshContext(computations.Where(x => x.Engine == computation.Engine));
        }

        var result = await computationEngine.Evaluate<double>(computation);
        return new EvaluateComputationResponse
        {
            Result = result
        };
    }
}

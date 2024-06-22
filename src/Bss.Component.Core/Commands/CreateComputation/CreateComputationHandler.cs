using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.CreateComputation;

//[Authorize(Roles = "Admin")]
public class CreateComputationHandler(
    IUserContext userContext,
    ILogger<CreateComputationHandler> logger,
    ILocalCacheCollection<Computation> computationCacheCollection,
    ICoreDbContext dbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory)
{
    public async Task Handle(CreateComputationRequest request)
    {
        var computationAnalyzer = computationAnalyzerFactory.GetService(request.Engine);

        var computation = new Computation(
            request.Name,
            request.Type,
            request.Engine,
            userContext.UserId);

        await computation.SetFormula(request.Formula, computationAnalyzer);

        dbContext.Push(computation);

        await dbContext.SaveChangesAsync();

        computationCacheCollection.Clear();
    }
}

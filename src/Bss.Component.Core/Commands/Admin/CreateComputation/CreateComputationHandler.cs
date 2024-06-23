using Bss.Component.Core.Data;
using Bss.Component.Core.Events.ComputationListChange;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.CreateComputation;

[Authorize(Roles = "Admin")]
public class CreateComputationHandler(
    IMediator mediator,
    IUserContext userContext,
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
            userContext.UserId,
            request.Disabled);

        await computation.SetFormula(request.Formula, computationAnalyzer);

        dbContext.Push(computation);

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(computation.Engine));
    }
}

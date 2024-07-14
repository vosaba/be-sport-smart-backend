using Bss.Component.Core.Data;
using Bss.Component.Core.Events.ComputationListChange;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.CreateComputations;

[Authorize(Roles = "Admin")]
public class CreateComputationsHandler(
    IMediator mediator,
    IUserContext userContext,
    ICoreDbContext dbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory)
{
    public async Task Handle(CreateComputationsRequest request)
    {
        foreach (var computationRequest in request.Computations)
        {
            var computationAnalyzer = computationAnalyzerFactory.GetService(request.Engine);

            var computation = new Computation(
                computationRequest.Name,
                computationRequest.Type,
                request.Engine,
                userContext.UserId,
                request.Disabled,
                computationRequest.Availability);

            await computation.SetFormula(computationRequest.Formula, computationAnalyzer);

            dbContext.Push(computation);

            await computation.SetFormula(computationRequest.Formula, computationAnalyzer);

            dbContext.Push(computation);
        }

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(request.Engine));
    }
}

using Bss.Component.Core.Data;
using Bss.Component.Core.Events.ComputationListChange;
using Bss.Component.Core.Data.Models;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.UpdateComputation;

[Authorize(Roles = "Admin")]
public class UpdateComputationHandler(
    IMediator mediator,
    IUserContext userContext,
    ICoreDbContext dbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory)
{
    public async Task Handle(UpdateComputationRequest request)
    {
        var computation = dbContext.Computations
            .SingleOrDefault(x => x.Id == request.Id);

        if (computation is null)
        {
            throw new NotFoundException<Computation>(request.Id);
        }
        else if (computation.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Computation is owned by another user.", OperationErrorCodes.Forbidden);
        }

        computation.Update(request.Type, request.Engine, request.Name, request.Disabled, request.Availability);

        var computationAnalyzer = computationAnalyzerFactory.GetService(computation.Engine);

        await computation.SetFormula(request.Formula, computationAnalyzer);

        await dbContext.SaveChangesAsync();

        await mediator.Publish(new ComputationListChangeEvent(computation.Engine));
    }
}

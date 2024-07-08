using Bss.Component.Core.Data;
using Bss.Component.Core.Events.ComputationListChange;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.UpdateComputations;

[Authorize(Roles = "Admin")]
public class UpdateComputationsHandler(
    IMediator mediator,
    IUserContext userContext,
    ICoreDbContext dbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory)
{
    public async Task Handle(UpdateComputationsRequest request)
    {
        foreach (var computationUpdate in request.Computations)
        {

            var computation = dbContext.Computations
                .SingleOrDefault(x => x.Id == computationUpdate.Id);

            if (computation is null)
            {
                throw new NotFoundException<Computation>(computationUpdate.Id);
            }
            else if (computation.CreatedBy != userContext.UserId)
            {
                throw new OperationException("Computation is owned by another user.", OperationErrorCodes.Forbidden);
            }

            computation.Update(computationUpdate.Type, computationUpdate.Engine, computationUpdate.Name, computationUpdate.Disabled);

            var computationAnalyzer = computationAnalyzerFactory.GetService(computation.Engine);

            await computation.SetFormula(computationUpdate.Formula, computationAnalyzer);
        }

        await dbContext.SaveChangesAsync();

        request.Computations.Select(x => x.Engine).Distinct().ToList().ForEach(async engine =>
        {
            await mediator.Publish(new ComputationListChangeEvent(engine));
        });
    }
}

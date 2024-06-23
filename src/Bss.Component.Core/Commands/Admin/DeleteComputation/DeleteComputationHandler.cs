using Bss.Component.Core.Data;
using Bss.Component.Core.Events.ComputationListChange;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.DeleteComputation;

[Authorize(Roles = "Admin")]
public class DeleteComputationHandler(IUserContext userContext, ICoreDbContext dbContext, IMediator mediator)
{
    public async Task Handle(DeleteComputationRequest request)
    {
        var computation = await dbContext
            .Computations
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (computation is null)
        {
            throw new NotFoundException<Measure>(request.Id);
        }

        if (computation.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Consumption is owned by another user.", OperationErrorCodes.Forbidden);
        }

        var consumingComputations = await dbContext
            .Computations
            .Where(x => x.RequiredComputations.Contains(computation.Name))
            .ToListAsync();

        if (consumingComputations.Count != 0)
        {
            throw new OperationException(
                "Computation is used by other computations.",
                consumingComputations.Select(x => x.Name).ToArray(),
                OperationErrorCodes.Conflict);
        }

        dbContext.Delete(computation);

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(computation.Engine));
    }
}

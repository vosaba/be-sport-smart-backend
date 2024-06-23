using Bss.Component.Core.Data;
using Bss.Component.Core.Events.MeasureListChange;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.DeleteMeasure;

[Authorize(Roles = "Admin")]
public class DeleteMeasureHandler(IUserContext userContext, ICoreDbContext dbContext, IMediator mediator)
{
    public async Task Handle(DeleteMeasureRequest request)
    {
        var measure = await dbContext
            .Measures
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (measure is null)
        {
            throw new NotFoundException<Measure>(request.Id);
        }

        if (measure.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Measure is owned by another user.", OperationErrorCodes.Forbidden);
        }

        var consumingComputations = await dbContext
            .Computations
            .Where(x => x.RequiredMeasures.Contains(measure.Name))
            .ToListAsync();

        if (consumingComputations.Count != 0)
        {
            throw new OperationException(
                "Measure is used in computations.",
                consumingComputations.Select(x => x.Name).ToArray(),
                OperationErrorCodes.Conflict);
        }

        dbContext.Delete(measure);

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new MeasureListChangeEvent());
    }
}

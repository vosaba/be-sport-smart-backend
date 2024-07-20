using Bss.Core.Admin.Events.MeasureListChange;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.UpdateMeasure;

[Authorize(Roles = "Admin")]
public class UpdateMeasureHandler(IUserContext userContext, ICoreDbContext dbContext, IMediator mediator)
{
    public async Task Handle(UpdateMeasureRequest request)
    {
        var measure = dbContext.Measures
            .SingleOrDefault(x => x.Id == request.Id);

        if (measure is null)
        { 
            throw new NotFoundException<Measure>(request.Id);
        }
        else if (measure.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Measure is owned by another user.", OperationErrorCodes.Forbidden);
        }
        
        measure.Update(
            request.Name,
            request.Type,
            request.MinValue,
            request.MaxValue,
            request.Availability,
            request.Options,
            request.Disabled,
            request.Order);

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new MeasureListChangeEvent());
    }
}

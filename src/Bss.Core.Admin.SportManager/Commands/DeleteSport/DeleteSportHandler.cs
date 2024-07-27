using Bss.Core.Admin.Events;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportManager.Commands.DeleteSport;

[Authorize(Roles = "Admin")]
public class DeleteSportHandler(
    IMediator mediator,
    IUserContext userContext,
    ICoreDbContext coreDbContext)
{
    public async Task Handle(DeleteSportRequest request)
    {
        var computation = await coreDbContext
            .Computations
            .Where(x => x.Type == ComputationType.Sport && request.Name == x.Name)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException(request.Name, nameof(Computation));

        if (!computation.IsEditableByUser(userContext.UserId, userContext.IsInRole))
        {
            throw new OperationException("Computation is owned by another user.", OperationErrorCodes.Forbidden);
        }

        coreDbContext.Delete(computation);

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));
    }
}
using Bss.Component.Core.Data;
using Bss.Component.Core.Events.MeasureListChange;
using Bss.Component.Core.Data.Models;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.CreateMeasures;

[Authorize(Roles = "Admin")]
public class CreateMeasuresHandler(IUserContext userContext, ICoreDbContext dbContext, IMediator mediator)
{
    public async Task Handle(CreateMeasuresRequest request)
    {
        foreach (var createMeasure in request.Measures)
        {
            var measure = new Measure(
                createMeasure.Name,
                createMeasure.Type,
                createMeasure.MinValue,
                createMeasure.MaxValue,
                createMeasure.Availability,
                userContext.UserId,
                createMeasure.Disabled,
                createMeasure.Options,
                createMeasure.Order);

            dbContext.Push(measure);
        }   

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new MeasureListChangeEvent());
    }
}

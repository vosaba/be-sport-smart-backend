﻿using Bss.Core.Admin.Events;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Identity.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.CreateMeasure;

[Authorize(Roles = "Admin")]
public class CreateMeasureHandler(IUserContext userContext, ICoreDbContext dbContext, IMediator mediator)
{
    public async Task Handle(CreateMeasureRequest request)
    {
        var measure = new Measure(
            request.Name,
            request.Type,
            request.MinValue,
            request.MaxValue,
            request.Availability,
            userContext.UserId,
            request.Disabled,
            request.Options,
            request.Order);

        dbContext.Push(measure);

        await dbContext.SaveChangesAsync();
        await mediator.Publish(new MeasureListChangeEvent());
    }
}

using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.CreateMeasure;

[Authorize(Roles = "Admin")]
public class CreateMeasureHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task Handle(CreateMeasureRequest request)
    {
        var measure = new Measure(
            request.Name,
            request.Type,
            request.InputSource,
            userContext.UserId,
            request.Disabled);

        dbContext.Push(measure);

        await dbContext.SaveChangesAsync();
    }
}

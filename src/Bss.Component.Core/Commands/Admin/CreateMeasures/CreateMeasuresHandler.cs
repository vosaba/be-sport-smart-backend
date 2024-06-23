using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.Admin.CreateMeasures;

[Authorize(Roles = "Admin")]
public class CreateMeasuresHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task Handle(CreateMeasuresRequest request)
    {
        foreach (var createMeasure in request.Measures)
        {
            var measure = new Measure(
                createMeasure.Name,
                createMeasure.Type,
                createMeasure.InputSource,
                userContext.UserId,
                createMeasure.Disabled);

            dbContext.Push(measure);
        }   

        await dbContext.SaveChangesAsync();
    }
}

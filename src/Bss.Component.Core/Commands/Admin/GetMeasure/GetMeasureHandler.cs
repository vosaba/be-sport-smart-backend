using Bss.Component.Core.Data;
using Bss.Component.Core.Dto;
using Bss.Component.Core.Data.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.GetMeasure;

[Authorize(Roles = "Admin")]
public class GetMeasureHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task<MeasureDto> Handle(GetMeasureRequest request)
    {
        var measure = await dbContext
            .Measures
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (measure is null)
        {
            throw new NotFoundException<Measure>(request.Id);
        }

        return new MeasureDto
        {
            Id = measure.Id,
            Name = measure.Name,
            Type = measure.Type,
            MinValue = measure.MinValue,
            MaxValue = measure.MaxValue,
            Order = measure.Order,
            Availability = measure.Availability,
            Disabled = measure.Disabled,
            Options = measure.Options
        };

    }
}

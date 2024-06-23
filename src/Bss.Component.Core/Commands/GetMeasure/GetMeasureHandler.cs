using Bss.Component.Core.Data;
using Bss.Component.Core.Dto;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.GetMeasure;

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
        else if (measure.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Measure is owned by another user.", OperationErrorCodes.Forbidden);
        }

        return new MeasureDto
        {
            Id = measure.Id,
            Name = measure.Name,
            Type = measure.Type,
            InputSource = measure.InputSource,
            Disabled = measure.Disabled,
            Options = measure.Options
        };

    }
}

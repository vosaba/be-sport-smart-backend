using Bss.Component.Core.Data;
using Bss.Component.Core.Dto;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.GetMeasures;

[Authorize(Roles = "Admin")]
public class GetMeasuresHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task<MeasureDto[]> Handle(GetMeasuresRequest request)
    {
        var measures = dbContext.Measures
            .Where(x => x.CreatedBy == userContext.UserId);

        if (request.Name is not null)
        {
            measures = measures.Where(x => x.Name.Contains(request.Name));
        }
        if (request.Type is not null)
        {
            measures = measures.Where(x => x.Type == request.Type);
        }
        if (request.InputSource is not null)
        {
            measures = measures.Where(x => x.InputSource == request.InputSource);
        }
        if (request.Disabled is not null)
        {
            measures = measures.Where(x => x.Disabled == request.Disabled);
        }

        return (await measures.ToListAsync())
            .Select(x => new MeasureDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                InputSource = x.InputSource,
                Disabled = x.Disabled,
                Options = x.Options
            }).ToArray();
    }
}

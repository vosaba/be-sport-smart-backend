using Bss.Core.Bl.Data;
using Bss.Core.Admin.Dto;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.GetComputations;

[Authorize(Roles = "Admin")]
public class GetComputationsHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task<ComputationDto[]> Handle(GetComputationsRequest request)
    {
        var computations = dbContext.Computations;

        if (request.Name is not null)
        {
            computations = computations.Where(x => x.Name.Contains(request.Name));
        }
        if (request.Engine is not null)
        {
            computations = computations.Where(x => x.Engine == request.Engine);
        }
        if (request.Type is not null)
        {
            computations = computations.Where(x => x.Type == request.Type);
        }
        if (request.Disabled is not null)
        {
            computations = computations.Where(x => x.Disabled == request.Disabled);
        }
        
        return (await computations.ToListAsync())
            .Select(x => new ComputationDto
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Engine = x.Engine,
                Formula = x.Formula,
                Disabled = x.Disabled,
                RequiredComputations = [.. x.RequiredComputations],
                RequiredMeasures = [.. x.RequiredMeasures]
            }).ToArray();
    }
}

using Bss.Core.Bl.Data;
using Bss.Core.Admin.Dto;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.Admin.GetComputation;

[Authorize(Roles = "Admin")]
public class GetComputationHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task<ComputationDto> Handle(GetComputationRequest request)
    {
        var computation = await dbContext
            .Computations
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (computation is null)
        {
            throw new NotFoundException<Computation>(request.Id);
        }

        return new ComputationDto
        {
            Id = computation.Id,
            Name = computation.Name,
            Type = computation.Type,
            Engine = computation.Engine,
            Formula = computation.Formula,
            Disabled = computation.Disabled,
            RequiredComputations = [..computation.RequiredComputations],
            RequiredMeasures = [..computation.RequiredMeasures]
        };
    }
}

using Bss.Core.Admin.SportManager.Dto;
using Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportManager.Commands.GetSports;

[Authorize(Roles = "Admin")]
public class GetSportsHandler(
    ICoreDbContext coreDbContext,
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    public async Task<SportDto[]> Handle(GetSportsRequest request)
    {
        var sportScoreDataQuery = coreDbContext.Computations
            .Where(x => x.Type == ComputationType.Sport && x.Engine == ComputationEngine.Js);

        if (!string.IsNullOrEmpty(request.Name))
        {
            sportScoreDataQuery = sportScoreDataQuery.Where(x => x.Name == request.Name);
        }

        var sportScoreData = await sportScoreDataQuery.ToListAsync();

        return sportScoreData
            .OrderBy(x => x.Name)
            .Select(x => new SportDto
            {
                Name = x.Name,
                Variables = sportFormulaManipulatorFactory
                    .GetService(x.Engine)
                    .GetFormulaVariables(x.Formula),
                Formula = x.Formula,
                Disabled = x.Disabled,
            }).ToArray();
    }
}

using Bss.Core.Admin.SportScore.Dto;
using Bss.Core.Admin.SportScore.Services.SportFormulaManipulator;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportScore.Commands.GetSportScoreData;

[Authorize(Roles = "Admin")]
public class GetSportScoreDataHandler(
    ICoreDbContext coreDbContext,
    IServiceFactory<ISportFormulaManipulator> formulaScoreManipulatorFactory)
{
    public async Task<SportScoreDto[]> Handle(GetSportScoreDataRequest request)
    {
        var sportScoreDataQuery = coreDbContext.Computations
            .Where(x => x.Type == ComputationType.Sport && x.Engine == ComputationEngine.Js);

        if (!string.IsNullOrEmpty(request.SportName))
        {
            sportScoreDataQuery = sportScoreDataQuery.Where(x => x.Name == request.SportName);
        }

        var sportScoreData = await sportScoreDataQuery.ToListAsync();

        return sportScoreData
            .OrderBy(x => x.Name)
            .Select(x => new SportScoreDto
            {
                SportName = x.Name,
                SportScoreData = formulaScoreManipulatorFactory
                    .GetService(x.Engine)
                    .GetSportScoreData(x)
            }).ToArray();
    }
}

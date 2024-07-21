using Bss.Core.Admin.Events;
using Bss.Core.Admin.SportScore.Dto;
using Bss.Core.Admin.SportScore.Services.SportFormulaManipulator;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportScore.Commands.UpdateSportScoreData;

[Authorize(Roles = "Admin")]
public class UpdateSportScoreDataHandler(
    IMediator mediator,
    ICoreDbContext coreDbContext,
    IServiceFactory<ISportFormulaManipulator> formulaScoreManipulatorFactory)
{
    public async Task Handle(SportScoreDto[] sportScores)
    {
        var names = sportScores.Select(x => x.SportName).ToArray();

        var sportScoreDataQuery = await coreDbContext.Computations
            .Where(
                x => x.Type == ComputationType.Sport
                && x.Engine == ComputationEngine.Js
                && names.Contains(x.Name))
            .ToListAsync();

        foreach (var sportScore in sportScores)
        {
            var sportScoreData = sportScoreDataQuery.FirstOrDefault(x => x.Name == sportScore.SportName) 
                ?? throw new NotFoundException(sportScore.SportName, nameof(SportScoreDto));

            var sportFormulaManipulator = formulaScoreManipulatorFactory
                .GetService(sportScoreData.Engine);

            await sportFormulaManipulator
                .ApplyScoreDataToFormula(sportScoreData, sportScore.SportScoreData);
        }

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));
    }
}

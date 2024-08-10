using Bss.Core.Admin.Events;
using Bss.Core.Admin.Services.ComputationAnalyzers;
using Bss.Core.Admin.SportManager.Dto;
using Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportManager.Commands.SyncSports;

[Authorize(Roles = "Admin")]
public class SyncSportsHandler(
    IMediator mediator,
    ICoreDbContext coreDbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory,
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    public async Task<List<SportDto>> Handle(SyncSportsRequest request)
    {
        var computations = await coreDbContext
            .Computations
            .Where(x => request.Names.Contains(x.Name))
            .ToListAsync();

        if (computations == null || !computations.Any())
        {
            throw new NotFoundException(string.Join(", ", request.Names), nameof(SportDto));
        }

        var sportDtos = new List<SportDto>();

        foreach (var computation in computations)
        {
            var computationAnalyzer = computationAnalyzerFactory.GetService(computation.Engine);
            var sportFormulaManipulator = sportFormulaManipulatorFactory.GetService(computation.Engine);

            var currentSportVariables = sportFormulaManipulator.GetFormulaVariables(computation.Formula);
            var newFormula = sportFormulaManipulator.CreateFormulaUsingData(currentSportVariables);

            await computation.SetFormula(
                newFormula,
                computationAnalyzer.GetComputationRequirements,
                computationAnalyzer.EnsureValid);

            sportDtos.Add(new SportDto
            {
                Name = computation.Name,
                Type = computation.Type,
                Variables = sportFormulaManipulator.GetFormulaVariables(computation.Formula),
                Formula = computation.Formula,
                Disabled = computation.Disabled,
            });
        }

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));

        return sportDtos;
    }
}

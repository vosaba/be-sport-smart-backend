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

namespace Bss.Core.Admin.SportManager.Commands.SyncSport;

[Authorize(Roles = "Admin")]
public class SyncSportHandler(
    IMediator mediator,
    ICoreDbContext coreDbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory,
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    public async Task<SportDto> Handle(SyncSportRequest request)
    {
        var computation = await coreDbContext
            .Computations
            .Where(x => x.Type == ComputationType.Sport && request.Name == x.Name)
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(request.Name, nameof(SportDto));

        var computationAnalyzer = computationAnalyzerFactory
            .GetService(computation.Engine);
        var sportFormulaManipulator = sportFormulaManipulatorFactory
            .GetService(computation.Engine);

        var newFormula = sportFormulaManipulator
            .CreateFormulaUsingData(request.Variables);

        await computation.SetFormula(
            newFormula,
            computationAnalyzer.GetComputationRequirements,
            computationAnalyzer.EnsureValid);

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));

        return new SportDto
        {
            Name = computation.Name,
            Variables = sportFormulaManipulator.GetFormulaVariables(computation.Formula),
            Formula = computation.Formula,
            Disabled = computation.Disabled,
        };
    }
}

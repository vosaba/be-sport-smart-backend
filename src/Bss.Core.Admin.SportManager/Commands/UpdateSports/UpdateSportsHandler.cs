﻿using Bss.Core.Admin.Events;
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

namespace Bss.Core.Admin.SportManager.Commands.UpdateSports;

[Authorize(Roles = "Admin")]
public class UpdateSportsHandler(
    IMediator mediator,
    ICoreDbContext coreDbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory,
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    public async Task Handle(UpdateSportsRequest request)
    {
        var names = request.Sports.Select(x => x.Name).ToArray();

        var computationsQuery = await coreDbContext.Computations
            .Where(
                x => x.Type == ComputationType.Sport
                && names.Contains(x.Name))
            .ToListAsync();

        foreach (var sportScore in request.Sports)
        {
            var computation = computationsQuery.FirstOrDefault(x => x.Name == sportScore.Name) 
                ?? throw new NotFoundException(sportScore.Name, nameof(SportDto));

            var computationAnalyzer = computationAnalyzerFactory
                .GetService(computation.Engine);
            var sportFormulaManipulator = sportFormulaManipulatorFactory
                .GetService(computation.Engine);

            var newFormula = sportFormulaManipulator
                .ApplyVariablesToFormula(computation.Formula, sportScore.Variables);

            await computation.SetFormula(
                newFormula,
                computationAnalyzer.GetComputationRequirements,
                computationAnalyzer.EnsureValid);
        }

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));
    }
}
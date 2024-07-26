﻿using Bss.Core.Admin.Events;
using Bss.Core.Admin.Services.ComputationAnalyzers;
using Bss.Core.Admin.SportManager.Dto;
using Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;
using Bss.Core.Bl.Data;
using Bss.Core.Bl.Enums;
using Bss.Core.Bl.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Core.Admin.SportManager.Commands.CreateSport;

[Authorize(Roles = "Admin")]
public class CreateSportHandler(
    IMediator mediator,
    IUserContext userContext,
    ICoreDbContext coreDbContext,
    IServiceFactory<IComputationAnalyzer> computationAnalyzerFactory,
    IServiceFactory<ISportFormulaManipulator> sportFormulaManipulatorFactory)
{
    public async Task Handle(CreateSportRequest request)
    {
        var computation = await coreDbContext
            .Computations
            .Where(x => x.Type == ComputationType.Sport && request.Sport == x.Name)
            .FirstOrDefaultAsync();

        if (computation != null)
        { 
            throw new OperationException($"Sport {request.Sport} already exists.");
        }

        var computationAnalyzer = computationAnalyzerFactory
            .GetService(request.ComputationEngine);
        var sportFormulaManipulator = sportFormulaManipulatorFactory
            .GetService(request.ComputationEngine);

        var newComputation = new Computation(
            request.Sport,
            ComputationType.Sport,
            request.ComputationEngine,
            userContext.UserId,
            request.Disabled);

        var newFormula = sportFormulaManipulator
            .CreateFormulaUsingData(request.Variables);

        await newComputation.SetFormula(
            newFormula,
            computationAnalyzer.GetComputationRequirements,
            computationAnalyzer.EnsureValid);

        await coreDbContext.SaveChangesAsync();
        await mediator.Publish(new ComputationListChangeEvent(ComputationEngine.Js));
    }
}
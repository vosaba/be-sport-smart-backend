using Bss.Core.Bl.Models;
using Bss.Core.Engine.Dto;
using Bss.Core.Engine.Services.ComputationRequirements;
using Bss.Core.Engine.Services.MeasureValues;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Commands.Computations.GetAvailableComputations;

[AllowAnonymous]
public class GetAvailableComputationsHandler(
    IUserContext userContext,
    ILogger<GetAvailableComputationsHandler> logger,
    IMeasureValueService measureValueService,
    IComputationRequirementService computationRequirementService,
    ILocalCacheCollection<Computation> computationCacheCollection)
{
    public async Task<AvailableComputationDto[]> Handle(GetAvailableComputationsRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");
        }

        var availableComputations = computationCacheCollection
            .GetAll()
            .Where(x =>
                x.Type == request.Type
                && x.IsExecutableByUser(userContext.IsAuthenticated));

        if (request.MeasureValues is not null)
        {
            var measureValues = measureValueService
                .GetValidMeasureValues(request.MeasureValues)
                .ToArray();

            availableComputations = computationRequirementService
                .FilterOutComputationsWithMissingMeasures(availableComputations, measureValues);
        }

        return availableComputations
            .Select(x => new AvailableComputationDto
            {
                Name = x.Name,
                Type = x.Type,
                Availability = x.Availability
            }).ToArray();
    }
}

using Bss.Component.Core.Dto;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services;
using Bss.Component.Core.Services.ComputationRequirements;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.GetAvailableComputations;

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

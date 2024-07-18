using Bss.Component.Core.Dto;
using Bss.Component.Core.Data.Models;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.Measures.GetAvailableMeasures;

[AllowAnonymous]
public class GetAvailableMeasuresHandler(
    IUserContext userContext,
    ILogger<GetAvailableMeasuresHandler> logger,
    ILocalCacheCollection<Measure> measuresCacheCollection)
{
    public async Task<AvailableMeasureDto[]> Handle(GetAvailableMeasuresRequest _)
    {
        if (measuresCacheCollection.IsEmpty)
        {
            logger.LogWarning("Measures cache is empty.");
        }

        var availableMeasures = measuresCacheCollection
            .GetAll()
            .Where(x => x.IsVisibleForUser(userContext.IsAuthenticated));

        return availableMeasures
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Availability)
            .Select(x => new AvailableMeasureDto
            {
                Name = x.Name,
                Type = x.Type,
                MinValue = x.MinValue,
                MaxValue = x.MaxValue,
                Availability = x.Availability,
                MeasurableByUser = x.IsMeasurableByUser(userContext.IsAuthenticated),
                Options = x.Options,
            })
            .ToArray();
    }
}

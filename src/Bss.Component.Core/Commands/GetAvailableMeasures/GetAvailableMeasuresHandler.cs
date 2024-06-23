using Bss.Component.Core.Dto;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.GetAvailableMeasures;

[AllowAnonymous]
public class GetAvailableMeasuresHandler(
    ILogger<GetAvailableMeasuresHandler> logger,
    ILocalCacheCollection<Measure> measuresCacheCollection)
{
    public async Task<AvailableMeasureDto[]> Handle(GetAvailableMeasuresRequest request)
    {
        if (measuresCacheCollection.IsEmpty)
        {
            logger.LogWarning("Measures cache is empty.");
        }

        var availableMeasures = measuresCacheCollection
            .GetAll()
            .Where(x => request.Sources.Contains(x.InputSource));

        return availableMeasures
            .OrderBy(x => x.InputSource)
            .Select(x => new AvailableMeasureDto
            {
                Name = x.Name,
                Type = x.Type,
                InputSource = x.InputSource,
                Options = x.Options,
            })
            .ToArray();
    }
}

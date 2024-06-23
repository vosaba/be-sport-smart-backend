using Bss.Component.Core.Enums;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.GetExecutableComputations;

[AllowAnonymous]
public class GetExecutableComputationsHandler(
    ILogger<GetExecutableComputationsHandler> logger,
    ILocalCacheCollection<Computation> computationCacheCollection)
{
    public async Task<GetExecutableComputationsResponse> Handle(GetExecutableComputationsRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");
        }

        var availableComputations = computationCacheCollection
            .GetAll()
            .Where(x => x.RequiredMeasures.All(c => request.MeasureValues.ContainsKey(c)))
            .Where(x => x.Type == ComputationType.Sport || x.Type == ComputationType.Metric);

        return new GetExecutableComputationsResponse
        {
            ComputationNames = availableComputations.Select(x => x.Name).ToArray()
        };
    }
}

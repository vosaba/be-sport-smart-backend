using Bss.Component.Core.Dto;
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
    public async Task<ExecutableComputationDto[]> Handle(GetExecutableComputationsRequest request)
    {
        if (computationCacheCollection.IsEmpty)
        {
            logger.LogWarning("Computation cache is empty.");
        }

        var availableComputations = computationCacheCollection
            .GetAll()
            .Where(x => x.Type == request.Type);

        return availableComputations.Select(x => new ExecutableComputationDto
        {
            Name = x.Name,
            Type = x.Type,
            Executable = x.RequiredMeasures.All(c => request.MeasureValues.ContainsKey(c))
        }).ToArray();
    }
}

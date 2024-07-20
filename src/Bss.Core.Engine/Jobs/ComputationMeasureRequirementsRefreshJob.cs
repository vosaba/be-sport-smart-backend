using Bss.Core.Engine.Services.ComputationRequirements;
using Bss.Infrastructure.Jobs.Abstractions;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Jobs;

[Job(nameof(ComputationMeasureRequirementsRefreshJob))]
public class ComputationMeasureRequirementsRefreshJob(
    ILogger<ComputationMeasureRequirementsRefreshJob> logger,
    IComputationRequirementService computationRequirementService) 
    : IJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("Going to refresh computation requirements.");

        computationRequirementService.RefreshMeasureRequirementsCache();

        logger.LogTrace("Computation requirements refreshed.");
    }
}
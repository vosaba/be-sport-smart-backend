using Bss.Core.Bl.Events.ComputationsCacheRefreshed;
using Bss.Core.Engine.Jobs;
using Bss.Infrastructure.Jobs.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Engine.Events.Handlers.ComputationsCacheRefreshed;

internal class ComputationsCacheRefreshedEventHandler(
    IJobRunner jobRunner,
    ILogger<ComputationsCacheRefreshedEventHandler> logger) 
    : INotificationHandler<ComputationsCacheRefreshedEvent>
{
    public async Task Handle(ComputationsCacheRefreshedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogTrace("ComputationsCacheRefreshed event received.");

        jobRunner.Trigger<ComputationEnginesRefreshJob>();
        jobRunner.Trigger<ComputationMeasureRequirementsRefreshJob>();

        logger.LogTrace("ComputationsCacheRefreshed event handled.");
    }
}

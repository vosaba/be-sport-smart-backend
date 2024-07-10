using Bss.Component.Core.Jobs;
using Bss.Infrastructure.Jobs.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Events.ComputationsCacheRefreshed;

internal class ComputationsCacheRefreshedEventHandler(
    IJobRunner jobRunner,
    ILogger<ComputationsCacheRefreshedEventHandler> logger) 
    : INotificationHandler<ComputationsCacheRefreshedEvent>
{
    public async Task Handle(ComputationsCacheRefreshedEvent @event, CancellationToken cancellationToken)
    {
        logger.LogTrace("ComputationsCacheRefreshed event received.");

        jobRunner.Trigger<ComputationEnginesRefreshJob>();

        logger.LogTrace("ComputationsCacheRefreshed event handled.");
    }
}

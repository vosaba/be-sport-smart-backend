using Bss.Core.Admin.Jobs;
using Bss.Infrastructure.Jobs.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Admin.Events.ComputationListChange;

internal class ComputationListChangeEventHandler(
    IJobRunner jobRunner,
    ILogger<ComputationListChangeEventHandler> logger) 
    : INotificationHandler<ComputationListChangeEvent>
{
    public async Task Handle(ComputationListChangeEvent @event, CancellationToken cancellationToken)
    {
        logger.LogTrace("Computation list change event received.");

        jobRunner.Trigger<ComputationsCacheRefreshJob>();

        logger.LogTrace("Computation list change event handled.");
    }
}

using Bss.Core.Admin.Jobs;
using Bss.Infrastructure.Jobs.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bss.Core.Admin.Events.Handlers;

internal class MeasureListChangeEventHandler(
    IJobRunner jobRunner,
    ILogger<MeasureListChangeEventHandler> logger)
    : INotificationHandler<MeasureListChangeEvent>
{
    public async Task Handle(MeasureListChangeEvent @event, CancellationToken cancellationToken)
    {
        logger.LogTrace("Measure list change event received.");

        jobRunner.Trigger<MeasuresCacheRefreshJob>();

        logger.LogTrace("Measure list change event handled.");
    }
}

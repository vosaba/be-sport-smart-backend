using Bss.Infrastructure.Jobs.Abstractions;
using Hangfire;

namespace Bss.Infrastructure.Jobs.Services;

internal class JobRunner(IRecurringJobManager recurringJobManager) : IJobRunner
{
    public void Trigger<TJob>() where TJob : IJob
    {
        var jobType = typeof(TJob);

        var jobAttribute = Attribute.GetCustomAttribute(jobType, typeof(JobAttribute)) as JobAttribute 
            ?? throw new InvalidOperationException($"JobAttribute is not defined on {jobType.Name}");

        recurringJobManager.Trigger(jobAttribute.Name);
    }
}

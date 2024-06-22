using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Jobs.Configurations;
using Hangfire;
using System.Reflection;

namespace Bss.Infrastructure.Jobs.HangfirePlugin;

internal static class RecurringJobManagerExtensions
{
    private const string NeverCronExpression = "0 0 31 2 *";

    public static void Configure(this IRecurringJobManager recurringJobs, JobsConfiguration jobsConfig)
    {
        var jobTypes = JobFinder.GetJobTypes(jobsConfig);

        foreach (var jobType in jobTypes)
        {
            var jobName = jobType.GetCustomAttribute<JobAttribute>()?.Name;

            var cronJobConfig = jobsConfig.CronJobs.FirstOrDefault(j => j.Name == jobName);
            var startupJobConfig = jobsConfig.OnDemandJobs.FirstOrDefault(j => j.Name == jobName);

            var executeAsyncMethodInfo = jobType.GetMethod(nameof(IJob.ExecuteAsync));
            if (executeAsyncMethodInfo == null)
            {
                continue;
            }

            if (cronJobConfig != null)
            {
                recurringJobs.AddOrUpdate(
                    cronJobConfig.Name, 
                    new Hangfire.Common.Job(jobType, executeAsyncMethodInfo, CancellationToken.None), 
                    cronJobConfig.CronExpression);
            }
            else if (startupJobConfig != null)
            {
                recurringJobs.AddOrUpdate(
                    startupJobConfig.Name,
                    new Hangfire.Common.Job(jobType, executeAsyncMethodInfo, CancellationToken.None),
                    NeverCronExpression);

                if (startupJobConfig.TriggerOnStartup)
                {
                    recurringJobs.Trigger(startupJobConfig.Name);
                }
            }
        }
    }
}

using Bss.Infrastructure.Configuration.Abstractions;
using Bss.Infrastructure.Jobs.Abstractions;
using Bss.Infrastructure.Jobs.Configurations;
using Bss.Infrastructure.Shared;
using System.Reflection;

namespace Bss.Infrastructure.Jobs;

public static class JobFinder
{
    public static IEnumerable<Type> GetJobTypes(JobsConfiguration jobsConfiguration)
    {
        var cronJobNames = jobsConfiguration.CronJobs.Select(x => x.Name).ToArray();
        var startupJobNames = jobsConfiguration.OnDemandJobs.Select(x => x.Name).ToArray();
        var allJobNames = cronJobNames.Union(startupJobNames).ToArray();

        var jobTypes = AssemblyManager.Instance
            .DiscoverTypes(x => x.GetTypeInfo().GetCustomAttribute<JobAttribute>() != null);
        
        var matchingTypes = jobTypes.Where(t =>
        {
            var attribute = t.GetCustomAttribute<JobAttribute>();
            return attribute != null && allJobNames.Contains(attribute.Name);
        });

        return matchingTypes;
    }
}
using Bss.Infrastructure.Configuration.Abstractions;

namespace Bss.Infrastructure.Jobs.Configurations;

[Configuration]
public class JobsConfiguration
{
    public DashboardConfiguration Dashboard { get; set; } = new();

    public CronJob[] CronJobs { get; set; } = [];

    public OnDemandJob[] OnDemandJobs { get; set; } = [];

    public class CronJob
    {
        public string Name { get; set; } = string.Empty;

        public string CronExpression { get; set; } = string.Empty;
    }

    public class OnDemandJob
    {
        public string Name { get; set; } = string.Empty;

        public bool TriggerOnStartup { get; set; }
    }

    public class DashboardConfiguration
    {
        public bool Enabled { get; set; }

        public bool ReadOnly { get; set; }

        public string Path { get; set; } = string.Empty;

        public string[] Roles { get; set; } = [];
    }
}

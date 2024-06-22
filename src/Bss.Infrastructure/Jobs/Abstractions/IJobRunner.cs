namespace Bss.Infrastructure.Jobs.Abstractions;

public interface IJobRunner
{
    void Trigger<TJob>() where TJob : IJob;
}
namespace Bss.Infrastructure.Jobs.Abstractions;

public interface IJob
{
    public Task ExecuteAsync(CancellationToken cancellationToken);
}
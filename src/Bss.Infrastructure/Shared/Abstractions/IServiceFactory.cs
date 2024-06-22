namespace Bss.Infrastructure.Shared.Abstractions;

public interface IServiceFactory<TService>
{
    TService GetService(string name);

    TService GetService(object name);
}

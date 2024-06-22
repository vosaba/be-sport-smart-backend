using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Shared.Abstractions;

public interface IServiceFactoryBuilder<TService>
{
    IServiceFactoryBuilder<TService> Register<TImplementation>(string name)
        where TImplementation : TService;

    IServiceFactoryBuilder<TService> Register<TImplementation>(object name)
        where TImplementation : TService;

    public IServiceCollection Build();
}
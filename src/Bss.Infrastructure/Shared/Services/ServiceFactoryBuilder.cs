using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Shared.Services;

internal class ServiceFactoryBuilder<TService>(IServiceCollection services) : IServiceFactoryBuilder<TService>
{
    private readonly Dictionary<string, Type> _services = [];

    public IServiceFactoryBuilder<TService> Register<TImplementation>(string name)
        where TImplementation : TService
    {
        _services[name] = typeof(TImplementation);
        return this;
    }


    public IServiceFactoryBuilder<TService> Register<TImplementation>(object name)
        where TImplementation : TService
        => Register<TImplementation>(name.ToString()!);

    public IServiceCollection Build()
    {
        services.AddSingleton<IServiceFactory<TService>>(provider => new ServiceFactory<TService>(provider, _services));
        return services;
    }
}
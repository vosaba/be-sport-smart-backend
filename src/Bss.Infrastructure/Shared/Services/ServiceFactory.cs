using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Shared.Services;

internal class ServiceFactory<TService>(IServiceProvider serviceProvider, Dictionary<string, Type> services) : IServiceFactory<TService> 
{
    public TService GetService(string name)
    {
        if (services.TryGetValue(name, out var serviceType))
        {
            return (TService)serviceProvider.GetRequiredService(serviceType);
        }

        throw new ArgumentException($"Service with name '{name}' is not registered.");
    }

    public TService GetService(object name)
        => GetService(name.ToString()!);
}

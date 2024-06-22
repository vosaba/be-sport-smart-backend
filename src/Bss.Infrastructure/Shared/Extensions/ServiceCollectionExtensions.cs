using Bss.Infrastructure.Shared.Abstractions;
using Bss.Infrastructure.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceFactoryBuilder<TService> CreateNamedServiceBuilder<TService>(this IServiceCollection services)
    {
        return new ServiceFactoryBuilder<TService>(services);
    }
}
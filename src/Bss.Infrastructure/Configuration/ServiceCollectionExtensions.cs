using Bss.Infrastructure.Configuration.Abstractions;
using Bss.Infrastructure.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bss.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var configTypes = AssemblyManager.Instance
            .DiscoverTypes(x => x.GetTypeInfo().GetCustomAttribute<ConfigurationAttribute>() != null);

        var method = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethod(nameof(OptionsConfigurationServiceCollectionExtensions.Configure), [typeof(IServiceCollection), typeof(IConfiguration)]);

        Array.ForEach(configTypes, x =>
        {
            var configSection = configuration.GetSection(ConfigurationAttribute.GetSectionName(x));

            if (method != null)
            {
                var genericMethod = method.MakeGenericMethod(x);
                genericMethod.Invoke(null, [services, configSection]);
            }
        });

        return services;
    }
}
using Bss.Infrastructure.Configuration.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Bss.Infrastructure.Configuration;

public static class ConfigurationExtentions
{
    public static TConfiguration GetConfiguration<TConfiguration>(this IConfiguration configuration)
    {
        var sectionName = ConfigurationAttribute.GetSectionName<TConfiguration>();
        return configuration.GetSection(sectionName).Get<TConfiguration>() 
            ?? throw new InvalidOperationException($"Configuration section '{sectionName}' does not exist or could not be converted to {typeof(TConfiguration).Name}");
    }

    public static TConfiguration GetConfiguration<TConfiguration>(this IConfigurationRoot configuration) 
        => GetConfiguration<TConfiguration>(configuration as IConfiguration);
}
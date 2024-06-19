using System.Reflection;
using System.Text.RegularExpressions;

namespace Bss.Infrastructure.Configuration.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public partial class ConfigurationAttribute(string? section = null) : Attribute
{
    public string? Section { get; } = section;

    public static string GetSectionName(Type type)
    {
        var configurationAttribute = type
            .GetTypeInfo()
            .GetCustomAttribute<ConfigurationAttribute>();

        return string.IsNullOrEmpty(configurationAttribute?.Section)
            ? ConfigSuffix().Replace(type.Name, string.Empty)
            : configurationAttribute.Section;
    }

    public static string GetSectionName<T>() => GetSectionName(typeof(T));

    [GeneratedRegex("(?:Config|Configuration)$", RegexOptions.Compiled)]
    private static partial Regex ConfigSuffix();
}
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Bss.Infrastructure.Configuration;

public static partial class ConfigurationBuilderExtentions
{
    private const string ConfigFileSetting = "config";
    private const string ConfigFileName = "config.json,**/*.config.json";
    private static readonly char[] ConfigFileSeparator = [','];

    public static void Configure(this IConfigurationBuilder builder, string[] args, string contentPath)
    {
        var moduleConfig = BuildModuleConfig();

        var rawConfig = new ConfigurationBuilder().SetBasePath(contentPath);

        var configFiles = GetConfigFileNames(moduleConfig, contentPath);
        foreach (var c in configFiles)
        {
            rawConfig.AddJsonFile(c, optional: true, reloadOnChange: true);
        }

        rawConfig.AddCommandLine(args);
        rawConfig.AddEnvironmentVariables();

        var configurationRoot = rawConfig.Build();

        builder.AddConfiguration(configurationRoot);
    }

    private static IConfigurationRoot BuildModuleConfig()
    {
        var builder = new ConfigurationBuilder();
        builder.AddEnvironmentVariables();
        return builder.Build();
    }

    private static string[] GetConfigFileNames(IConfiguration moduleConfig, string basePath)
    {
        var configFile = moduleConfig[ConfigFileSetting];

        if (string.IsNullOrEmpty(configFile))
        {
            configFile = ConfigFileName;
        }

        var root = new DirectoryInfo(basePath);
        var result = new List<string>();

        var patterns = configFile.Split(ConfigFileSeparator, StringSplitOptions.RemoveEmptyEntries);
        foreach (var pattern in patterns)
        {
            var path = AllDirsMatch().Replace(pattern, "$1");
            var searchOptions = AllDirsMatch().IsMatch(pattern)
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = root.GetFiles(path, searchOptions)
                .OrderBy(x => x.DirectoryName).ThenBy(x => x.Name)
                .Select(x => x.FullName.Substring(basePath.Length).Trim(Path.DirectorySeparatorChar));

            result.AddRange(files);
        }

        return result.ToArray();
    }

    [GeneratedRegex(@"([/\\]|^)\*\*([/\\])", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex AllDirsMatch();
}
using Microsoft.Extensions.DependencyModel;
using System.Reflection;

namespace Bss.Infrastructure.Shared;

public class AssemblyManager
{
    private static readonly Lazy<Assembly[]> _assemblies = new(LoadAssemblies);

    public static AssemblyManager Instance { get; } = new AssemblyManager();

    public static IEnumerable<Assembly> GetAssemblies(Func<Assembly, bool>? predicate = null)
    {
        return predicate == null
            ? _assemblies.Value
            : _assemblies.Value.Where(predicate).ToArray();
    }

    private static Assembly[] LoadAssemblies()
    {
        var privateLibAsm = typeof(Exception).Assembly ?? throw new InvalidOperationException("Could not load the private library assembly.");
        var pablicLibAsm = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Could not load the public library assembly.");

        var dependencyContext = DependencyContext.Load(pablicLibAsm);
        return dependencyContext == null
            ? throw new InvalidOperationException("Could not load the dependency context.")
            : dependencyContext.RuntimeLibraries
                .SelectMany(l => l.GetDefaultAssemblyNames(dependencyContext))
                .Select(Assembly.Load)
                .Union([privateLibAsm])
                .ToArray();
    }
}
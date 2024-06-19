using System.Reflection;
using System.Runtime.InteropServices;

namespace Bss.Infrastructure.Shared;

/// <summary>
/// Extensions used for types discovery and methods invocation.
/// </summary>
public static class InvocationExtensions
{
    /// <summary>
    /// Iterates method parameters, looks up parameter values from passed dependencies
    /// and returns parameters array that is prepared for method invocation.
    /// </summary>
    /// <param name="method">method for future invocation.</param>
    /// <param name="dependencies">dependencies for method parameters lookup.</param>
    /// <returns>parameters array that is prepared for method invocation.</returns>
    public static object[] CollectMethodParameters(this MethodBase method, params object[] dependencies)
    {
        return method.CollectMethodParameters(null, dependencies);
    }

    /// <summary>
    /// Iterates method parameters, looks up parameter values from passed dependencies
    /// and returns parameters array that is prepared for method invocation.
    /// </summary>
    /// <param name="method">method for future invocation.</param>
    /// <param name="provider">services provider for parameters lookup.</param>
    /// <param name="dependencies">dependencies for method parameters lookup.</param>
    /// <returns>parameters array that is prepared for method invocation.</returns>
    public static object[] CollectMethodParameters(this MethodBase method, [Optional] IServiceProvider? provider, params object[] dependencies)
    {
        method.TryCollectMethodParameters(provider, dependencies, true, out var result);
        return result;
    }

    public static bool TryCollectMethodParameters(this MethodBase method, [Optional] IServiceProvider? provider, object[] dependencies, out object[] parameters)
    {
        return method.TryCollectMethodParameters(provider, dependencies, false, out parameters);
    }

    public static object CreateInstance(this Type type, params object[] dependencies)
    {
        return type.CreateInstance(null, dependencies);
    }

    public static object CreateInstance(this Type type, [Optional] IServiceProvider? serviceProvider, params object[] dependencies)
    {
        var ctors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).ToArray();

        foreach (var ctor in ctors)
        {
            if (ctor.TryCollectMethodParameters(serviceProvider, dependencies, out var parameters))
            {
                return ctor.Invoke(parameters);
            }
        }

        throw new MissingMemberException(
            $"Unable to find appropriate public .ctor for {type.FullName} and lookup all required dependencies for .ctor invocation");
    }

    public static T[] DiscoverTypesAndInstantiate<T>(this AssemblyManager manager, Func<Type, bool>? condition = null, IServiceProvider? serviceProvider = null, params object[] dependencies)
    {
        return AssemblyManager.Instance
            .DiscoverTypesForInstantiation<T>(condition)
            .Select(s => s.CreateInstance(serviceProvider, dependencies))
            .Cast<T>()
            .ToArray();
    }

    public static Type[] DiscoverTypesForInstantiation<T>(this AssemblyManager manager, Func<Type, bool>? condition = null)
    {
        return manager.DiscoverTypes(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract && (condition == null || condition(t)));
    }

    public static Type[] DiscoverTypes(this AssemblyManager manager, Func<Type, bool> condition)
    {
        var types = new List<Type>();
        foreach (var asm in AssemblyManager.GetAssemblies())
        {
            foreach (var t in asm.GetTypes())
            {
                if (condition(t))
                {
                    types.Add(t);
                }
            }
        }

        var result = types
            .OrderBy(t => t.Namespace)
            .Distinct()
            .ToArray();

        return result;
    }

    private static bool TryCollectMethodParameters(this MethodBase method, [Optional] IServiceProvider? provider, object[] dependencies, bool throwIfNotFound, out object[] parameters)
    {
        parameters = [];

        var methodParameters = method.GetParameters();
        var resultList = new List<object>(methodParameters.Length);

        foreach (var p in methodParameters)
        {
            var result = dependencies.FirstOrDefault(d => p.ParameterType.IsAssignableFrom(d.GetType()));
            if (result == null && provider != null)
            {
                result = provider.GetService(p.ParameterType);
            }

            if (result == null)
            {
                return throwIfNotFound
                    ? throw new KeyNotFoundException(
                            $"Unable to lookup parameter {p.Name}: {p.ParameterType} from the list of dependencies")
                    : false;
            }

            resultList.Add(result);
        }

        parameters = resultList.ToArray();

        return true;
    }
}
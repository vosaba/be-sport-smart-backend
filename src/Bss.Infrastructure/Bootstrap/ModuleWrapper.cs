using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Bss.Infrastructure.Bootstrap;

internal class ModuleWrapper
{
    private readonly object? _instance;
    private readonly MethodInfo? _configureMethod;
    private readonly MethodInfo? _configureServicesMethod;

    public ModuleWrapper(object? instance)
    {
        if (instance != null)
        {
            var moduleType = instance.GetType();

            _instance = instance;
            _configureMethod = moduleType.GetMethod("Configure", BindingFlags.Instance | BindingFlags.Public);
            _configureServicesMethod =
                moduleType.GetMethod("ConfigureServices", BindingFlags.Instance | BindingFlags.Public);
        }
    }

    public void Configure(IApplicationBuilder builder)
    {
        if (_configureMethod == null)
        {
            return;
        }

        // Create a scope for Configure, this allows creating scoped dependencies
        // without the hassle of manually creating a scope.
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var parameterInfos = _configureMethod.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (var index = 0; index < parameterInfos.Length; index++)
            {
                var parameterInfo = parameterInfos[index];
                if (parameterInfo.ParameterType == typeof(IApplicationBuilder))
                {
                    parameters[index] = builder;
                }
                else
                {
                    try
                    {
                        parameters[index] = serviceProvider.GetRequiredService(parameterInfo.ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            string.Format(
                                "Could not resolve a service of type '{0}' for the parameter '{1}' of method '{2}' on type '{3}'.",
                                parameterInfo.ParameterType.FullName,
                                parameterInfo.Name,
                                _configureMethod.Name,
                                _configureMethod.DeclaringType?.FullName),
                            ex);
                    }
                }
            }

            _configureMethod.Invoke(_instance, BindingFlags.DoNotWrapExceptions, null, parameters, CultureInfo.InvariantCulture);
        }
    }

    public void ConfigureApp(WebApplication builder)
    {
        if (_configureMethod == null)
        {
            return;
        }

        // Create a scope for Configure, this allows creating scoped dependencies
        // without the hassle of manually creating a scope.
        using var scope = builder.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var parameterInfos = _configureMethod.GetParameters();
        var parameters = new object[parameterInfos.Length];
        for (var index = 0; index < parameterInfos.Length; index++)
        {
            var parameterInfo = parameterInfos[index];
            if (parameterInfo.ParameterType == typeof(IApplicationBuilder))
            {
                parameters[index] = builder;
            }
            else
            {
                try
                {
                    parameters[index] = serviceProvider.GetRequiredService(parameterInfo.ParameterType);
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        string.Format(
                            "Could not resolve a service of type '{0}' for the parameter '{1}' of method '{2}' on type '{3}'.",
                            parameterInfo.ParameterType.FullName,
                            parameterInfo.Name,
                            _configureMethod.Name,
                            _configureMethod.DeclaringType?.FullName),
                        ex);
                }
            }
        }

        _configureMethod.Invoke(_instance, BindingFlags.DoNotWrapExceptions, null, parameters, CultureInfo.InvariantCulture);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_configureServicesMethod == null)
        {
            return;
        }

        _configureServicesMethod.Invoke(_instance, BindingFlags.DoNotWrapExceptions, null, [services], CultureInfo.InvariantCulture);
    }
}

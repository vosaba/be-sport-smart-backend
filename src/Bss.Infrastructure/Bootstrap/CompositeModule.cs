using Bss.Infrastructure.Bootstrap.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Bss.Infrastructure.Bootstrap;

internal class CompositeModule : ICompositeModule
{
    private readonly List<ModuleWrapper> _moduleWrappers = [];

    public CompositeModule(IConfiguration configuration, IHostEnvironment hostEnvironment, IEnumerable<Type> modules)
    {
        foreach (var module in modules)
        {
            var constructor = module.GetConstructors().First();
            var ctorParams = constructor.GetParameters();
            object? instance;

            if (ctorParams.Length != 0)
            {
                instance = CreateModuleInstanceWithAllowedInjections(
                    ctorParams,
                    new (Type, object)[]
                    {
                        (typeof(IConfiguration), configuration),
                        (typeof(IHostEnvironment), hostEnvironment)
                    },
                    module);
            }
            else
            {
                instance = Activator.CreateInstance(module);
            }

            _moduleWrappers.Add(new ModuleWrapper(instance));
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        foreach (var moduleWrapper in _moduleWrappers)
        {
            moduleWrapper.Configure(app);
        }
    }

    public void ConfigureApp(WebApplication app)
    {
        foreach (var moduleWrapper in _moduleWrappers)
        {
            moduleWrapper.ConfigureApp(app);
        }
    }

    public void ConfigureServices(IServiceCollection services)
    {
        foreach (var moduleWrapper in _moduleWrappers)
        {
            moduleWrapper.ConfigureServices(services);
        }
    }

    private static object? CreateModuleInstanceWithAllowedInjections(
        ParameterInfo[] ctorParams,
        (Type ServiceType, object Instance)[] allowedServicesToInject,
        Type moduleType)
    {
        var activationParams = new List<object>();

        foreach (var ctorParam in ctorParams)
        {
            var allowedInjection = allowedServicesToInject
                .Where(t => t.ServiceType == ctorParam.ParameterType)
                .Select(t => t.Instance)
                .FirstOrDefault();

            if (allowedInjection == null)
            {
                throw new ArgumentOutOfRangeException(
                    $"Module class should have only {string.Join(", ", allowedServicesToInject.Select(t => t.ServiceType.Name))} param(s) in {moduleType.FullName} constructor");
            }

            activationParams.Add(allowedInjection);
        }

        return Activator.CreateInstance(moduleType, activationParams.ToArray());
    }
}
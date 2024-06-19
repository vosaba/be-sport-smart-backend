using Bss.Infrastructure.Bootstrap;
using Bss.Infrastructure.Bootstrap.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.WebHost;

public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder UseCompositeModule(this IWebHostBuilder webBuilder, params Type[] modules)
    {
        webBuilder.ConfigureServices((context, collection) =>
        {
            var module = new CompositeModule(context.Configuration, context.HostingEnvironment, modules);
            module.ConfigureServices(collection);
            collection.AddSingleton<ICompositeModule>(module);
        });
        webBuilder.Configure((context, builder) =>
        {
            var module = builder.ApplicationServices.GetRequiredService<ICompositeModule>();
            module.Configure(builder);
        });

        return webBuilder;
    }

    public static void UseCompositeModuleConfigureServices(this WebApplicationBuilder webBuilder, params Type[] modules)
    {
        webBuilder.Host.ConfigureServices((context, collection) =>
        {
            var module = new CompositeModule(context.Configuration, context.HostingEnvironment, modules);
            module.ConfigureServices(collection);
            collection.AddSingleton<ICompositeModule>(module);
        });
    }

    public static void UseCompositeModuleConfigure(this WebApplication app)
    {
        var module = app.Services.GetService(typeof(ICompositeModule)) as ICompositeModule;
        module!.ConfigureApp(app);
    }
}
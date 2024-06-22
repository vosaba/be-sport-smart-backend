using Bss.Component.Core.Enums;
using Bss.Component.Core.Jobs;
using Bss.Component.Core.Services.ComputationAnalyzers;
using Bss.Component.Core.Services.ComputationEngines;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Component.Core;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<JsComputationEngine>();
        services.AddSingleton<DummyComputationEngine>();

        services.AddScoped<JsComputationAnalyzer>();
        services.AddScoped<DummyComputationAnalyzer>();

        services.CreateNamedServiceBuilder<IComputationEngine>()
            .Register<JsComputationEngine>(ComputationEngine.Js)
            .Register<DummyComputationEngine>(ComputationEngine.Dummy)
            .Build();

        services.CreateNamedServiceBuilder<IComputationAnalyzer>()
            .Register<JsComputationAnalyzer>(ComputationEngine.Js)
            .Register<DummyComputationAnalyzer>(ComputationEngine.Dummy)
            .Build();
        
        services.AddScoped<CacheInitializerJob>();
        services.AddScoped<ComputationEnginesInitializerJob>();

        services.AddCommands<Module>(nameof(Core));
        services.AddMediatR(typeof(Module).Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {        
    }
}

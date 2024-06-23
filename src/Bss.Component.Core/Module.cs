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
        
        services.AddScoped<MeasuresCacheRefreshJob>();
        services.AddScoped<ComputationsCacheRefreshJob>();
        services.AddScoped<ComputationEnginesRefreshJob>();

        services.AddCommands<Module>(nameof(Core), filters: [type => !IsAdminCommand(type)]);
        services.AddCommands<Module>(nameof(Core), "admin", [IsAdminCommand]);

        services.AddMediatR(typeof(Module).Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {        
    }

    private static bool IsAdminCommand(Type type) => type.Namespace!.Contains(nameof(Commands.Admin));
}

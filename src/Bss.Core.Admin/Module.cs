using Bss.Core.Admin.Jobs;
using Bss.Core.Admin.Services.ComputationAnalyzers;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Core.Admin;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<JsComputationAnalyzer>();
        services.AddScoped<DummyComputationAnalyzer>();

        services.CreateNamedServiceBuilder<IComputationAnalyzer>()
            .Register<JsComputationAnalyzer>(ComputationEngine.Js)
            .Register<DummyComputationAnalyzer>(ComputationEngine.Dummy)
            .Build();
        
        services.AddScoped<MeasuresCacheRefreshJob>();
        services.AddScoped<ComputationsCacheRefreshJob>();

        services.AddCommands<Module>(nameof(Core), "admin");

        services.AddMediatR(typeof(Module).Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {        
    }
}

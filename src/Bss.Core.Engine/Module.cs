using Bss.Core.Bl.Enums;
using Bss.Core.Engine.Jobs;
using Bss.Core.Engine.Services.ComputationEngines;
using Bss.Core.Engine.Services.ComputationRequirements;
using Bss.Core.Engine.Services.MeasureValues;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Core.Engine;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMeasureValueService, MeasureValueService>();
        services.AddSingleton<IComputationRequirementService, ComputationRequirementService>();

        services.AddSingleton<JsComputationEngine>();
        services.AddSingleton<DummyComputationEngine>();

        services.CreateNamedServiceBuilder<IComputationEngine>()
            .Register<JsComputationEngine>(ComputationEngine.Js)
            .Register<DummyComputationEngine>(ComputationEngine.Dummy)
            .Build();
        
        services.AddScoped<ComputationEnginesRefreshJob>();
        services.AddScoped<ComputationMeasureRequirementsRefreshJob>();

        services.AddCommands<Module>(nameof(Core));

        services.AddMediatR(typeof(Module).Assembly);
    }

    public void Configure(IApplicationBuilder app)
    {        
    }
}

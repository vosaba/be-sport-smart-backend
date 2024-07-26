using Bss.Core.Admin.SportManager.Services.SportFormulaManipulator;
using Bss.Core.Bl.Enums;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Core.Admin.SportManager;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<JsSportFormulaManipulator>();
        services.AddScoped<DummySportFormulaManipulator>();

        services.CreateNamedServiceBuilder<ISportFormulaManipulator>()
            .Register<JsSportFormulaManipulator>(ComputationEngine.Js)
            .Register<DummySportFormulaManipulator>(ComputationEngine.Dummy)
            .Build();

        services.AddCommands<Module>(nameof(Core), "admin/sportManager");
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}

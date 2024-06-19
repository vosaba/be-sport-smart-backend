using Bss.Component.Core.Services;
using Bss.Infrastructure.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Component.Core;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IComputationValidator, ComputationValidator>();
        services.AddCommands<Module>(nameof(Core));
    }

    public void Configure(IApplicationBuilder app)
    {        
    }
}

using Bss.Infrastructure.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.UserValues;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommands<Module>(nameof(UserValues));
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}

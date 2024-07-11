using Bss.Infrastructure.Commands;
using Bss.Infrastructure.WebHost.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bss.Infrastructure.WebHost;

internal class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddEndpointsApiExplorer();
    }

    public void Configure(IApplicationBuilder app, IOptions<SecurityConfiguration> securityConfig)
    {
        app.UseCors(x => x
            .WithOrigins(securityConfig.Value.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

        app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapCommands();
            endpoints.MapControllers();
        });
    }
}

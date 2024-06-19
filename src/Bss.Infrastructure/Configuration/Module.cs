using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Configuration;

internal class Module(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConfigurations(configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}
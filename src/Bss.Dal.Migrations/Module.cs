using Bss.Dal.Migrations.Initializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Dal.Migrations;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CoreDbContextInitializer>();
        services.AddScoped<IdentityDbContextInitializer>();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}

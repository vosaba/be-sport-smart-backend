using Bss.Dal.Migrations.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Dal.Migrations;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<RunMigrationsJob>();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}

using Bss.Dal.Migrations.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Bootstrap;

public class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
    }

    public async Task Configure(IApplicationBuilder app)
    {
        await app.RunMigrations();
    }
}
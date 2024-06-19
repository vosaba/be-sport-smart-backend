using Bss.Component.Core.Data;
using Bss.Component.Identity.Data;
using Bss.Component.Identity.Models;
using Bss.Dal.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bss.Dal;

public class Module
{
    private const string DbMigrationAssembly = "Bss.Dal.Migrations";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ICoreDbContext, CoreDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider
                .GetRequiredService<IOptions<BssDalConfiguration>>()
                .Value;

            options.UseNpgsql(
                configuration.ConnectionStrings.BssCore, 
                builder => builder
                    .SetPostgresVersion(new Version(configuration.PostgresVersion))
                    .MigrationsAssembly(DbMigrationAssembly));
        });

        services.AddDbContext<IIdentityDbContext, IdentityDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider
                .GetRequiredService<IOptions<BssDalConfiguration>>()
                .Value;

            options.UseNpgsql(configuration.ConnectionStrings.BssIdentity, 
                builder => builder
                    .SetPostgresVersion(new Version(configuration.PostgresVersion))
                    .MigrationsAssembly(DbMigrationAssembly));
        });

        services.AddIdentity<ApplicationUser, ApplicationUserRole>()
            .AddEntityFrameworkStores<IdentityDbContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}

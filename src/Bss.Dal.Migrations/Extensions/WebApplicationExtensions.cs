using Bss.Dal.Migrations.Initializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Dal.Migrations.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var coreInitializer = scope.ServiceProvider.GetRequiredService<CoreDbContextInitializer>();
        var identityInitializer = scope.ServiceProvider.GetRequiredService<IdentityDbContextInitializer>();

        await Task.WhenAll(coreInitializer.InitialiseAsync(), identityInitializer.InitialiseAsync());
    }
}

using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Identity;

internal class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserContext, UserContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}

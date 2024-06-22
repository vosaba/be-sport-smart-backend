using Bss.Infrastructure.Shared.Abstractions;
using Bss.Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Shared;

internal class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped(typeof(ILocalCacheCollection<>), typeof(LocalCacheCollection<>));
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}
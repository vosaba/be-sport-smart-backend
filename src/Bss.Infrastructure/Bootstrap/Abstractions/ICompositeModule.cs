using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Bootstrap.Abstractions;

public interface ICompositeModule
{
    void Configure(IApplicationBuilder app);

    void ConfigureApp(WebApplication app);

    void ConfigureServices(IServiceCollection services);
}
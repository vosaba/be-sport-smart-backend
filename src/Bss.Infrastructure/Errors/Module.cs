using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bss.Infrastructure.Errors;

internal class Module
{
    public void ConfigureServices(IServiceCollection services)
    {
        //services.AddExceptionHandler<ExceptionHandler>();
    }

    public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}

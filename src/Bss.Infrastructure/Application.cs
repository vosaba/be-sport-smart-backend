using Bss.Infrastructure.Configuration;
using Bss.Infrastructure.WebHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Bss.Infrastructure;

public class Application
{
    public static async Task Run(string[] args, params Type[] modules)
    {
        try
        {
            await using var host = CreateHostBuilder(args, modules).Build();
            host.UseCompositeModuleConfigure();
            await host.RunAsync();
        }
        catch (HostAbortedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ex));
            Environment.Exit(-87);
        }
    }

    internal static WebApplicationBuilder CreateHostBuilder(string[] args, params Type[] modules)
    {
        var contentPath = Directory.GetCurrentDirectory();
        var app = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = contentPath
        });
        app.Host.UseDefaultServiceProvider(o => o.ValidateScopes = false);
        app.Configuration.Configure(args, contentPath);
        app.WebHost.UseKestrel((builderContext, options) =>
        {
            var port = builderContext.Configuration.GetValue<int>("server:port", 8080);
            var protocol = builderContext.Configuration.GetValue<string>("server:protocol", "http");
            var certPath = builderContext.Configuration.GetValue<string>("server:tls:pfx");
            var pfxPassword = builderContext.Configuration.GetValue<string>("server:tls:pfxPassword");
            options.Listen(IPAddress.Any, port, listenOptions =>
            {
                if (string.Equals("https", protocol, StringComparison.OrdinalIgnoreCase))
                {
                    var pfxCert = File.ReadAllBytes(certPath!);
                    var cert = new X509Certificate2(pfxCert, pfxPassword);
                    listenOptions.UseHttps(cert);
                }
            });
        });
        app.UseCompositeModuleConfigureServices([..Modules.InfrastructureModules, ..modules]);

        return app;
    }
}

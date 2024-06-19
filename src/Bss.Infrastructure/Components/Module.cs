using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Components.Configurations;
using Bss.Infrastructure.Configuration;
using Bss.Infrastructure.Identity.Abstractions;
using Bss.Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Bss.Infrastructure.Components;

internal class Module(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting();
        services.AddTransient<IApiDescriptionProvider, ApiCommandDescriptionProvider>();

        var config = configuration.GetConfiguration<ComponentsConfiguration>().SecurityDefinition;

        services.AddControllers()
            .ConfigureApplicationPartManager(m => { m.ApplicationParts.Add(new ComponentsApplicationPart()); })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Culture = CultureInfo.InvariantCulture;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        services.AddSwaggerGen(builder =>
        {
            builder.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            builder.EnableAnnotations();

            foreach (var asmName in GetAssemblyNames())
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{asmName}.xml");
                if (File.Exists(xmlPath))
                {
                    builder.IncludeXmlComments(xmlPath);
                }
            }

            builder.SchemaGeneratorOptions.UseAllOfToExtendReferenceSchemas = true;
            builder.SchemaGeneratorOptions.UseOneOfForPolymorphism = false;

            if (config.EnabledSecurityDefinitions.HasFlag(SecurityDefinition.BearerJwt))
            {
                builder.AddSecurityDefinition(AuthenticationSchemes.BearerJwt, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT", // nameof(SecurityDefinition.Jwt)
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(config.JwtTokenProviderUrl),
                        }
                    },
                });
            }

            if (config.EnabledSecurityDefinitions.HasFlag(SecurityDefinition.ApiKey) || config.EnabledSecurityDefinitions.HasFlag(SecurityDefinition.Basic))
            {
                throw new NotImplementedException("SecurityDefinition.ApiKey and SecurityDefinition.Basic are not implemented yet");
            }
            
            builder.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bss API V1");
        });
    }

    private static IEnumerable<string?> GetAssemblyNames()
    {
        var assemblies = AssemblyManager.GetAssemblies();
        return assemblies.Select(s => s.GetName().Name).ToArray();
    }
}
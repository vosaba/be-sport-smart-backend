using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Bss.Infrastructure.Components;

internal class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // check is anonymous enabled
        if ((context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() ??
             context.MethodInfo.DeclaringType?.GetCustomAttribute<AllowAnonymousAttribute>()) != null)
        {
            return;
        }

        // check is authorize enabled
        if ((context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>() ??
            context.MethodInfo.GetCustomAttribute<AuthorizeAttribute>()) is null)
        {
            return;
        }

        // get authentication schemes
        var authenticationSchemesValue = context.MethodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>()?.AuthenticationSchemes;
        var schemas = string.IsNullOrEmpty(authenticationSchemesValue) ? [] : authenticationSchemesValue.Split(',');

        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

        operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Reference = new OpenApiReference
                            {
                                Id = AuthenticationSchemes.BearerJwt,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                },
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Reference = new OpenApiReference
                            {
                                Id = AuthenticationSchemes.ApiKey,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                },
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Reference = new OpenApiReference
                            {
                                Id = AuthenticationSchemes.Basic,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            }.Where(x => !schemas.Any() || x.Keys.Any(k => schemas.Contains(k.Reference.Id))).ToList();
    }
}
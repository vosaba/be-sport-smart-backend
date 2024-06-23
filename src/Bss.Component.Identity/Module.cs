using Bss.Component.Identity.Configuration;
using Bss.Component.Identity.Data;
using Bss.Component.Identity.Extensions;
using Bss.Component.Identity.Jobs;
using Bss.Component.Identity.Models;
using Bss.Component.Identity.Services;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Configuration;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bss.Component.Identity;

public class Module(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        var config = configuration.GetConfiguration<BssIdentityConfiguration>();

        services.AddHttpContextAccessor();

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme =
            option.DefaultChallengeScheme =
            option.DefaultScheme = AuthenticationSchemes.BearerJwt;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.Jwt.Issuer,
                ValidAudience = config.Jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Jwt.SigningKey)),
            };
        });

        services.AddIdentity<ApplicationUser, ApplicationUserRole>()
            .AddEntityFrameworkStores<ApplicationUser, ApplicationUserRole, IIdentityDbContext>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IdentityInitializerJob>();

        services.AddCommands<Module>(nameof(Identity));
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
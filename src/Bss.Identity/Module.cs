using Bss.Identity.Configuration;
using Bss.Identity.Data;
using Bss.Identity.Extensions;
using Bss.Identity.Jobs;
using Bss.Identity.Models;
using Bss.Identity.Services;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Configuration;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bss.Identity;

public class Module(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        var config = configuration.GetConfiguration<BssIdentityConfiguration>();

        services.AddHttpContextAccessor();

        var t = services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme =
            option.DefaultChallengeScheme =
            option.DefaultScheme = AuthenticationSchemes.BearerJwt;
        })
        .AddGitHub(githubOptions =>
        {
            githubOptions.ClientId = config.Github.ClientId;
            githubOptions.ClientSecret = config.Github.ClientSecret;
            githubOptions.CallbackPath = new PathString("/api/v1/identity/signInGithub");
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

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IdentityInitializerJob>();

        services.AddCommands<Module>(nameof(Identity));
    }

    public void Configure(IApplicationBuilder app)
    {
    }
}
using Bss.Identity.Configuration;
using Bss.Identity.Data;
using Bss.Identity.Enums;
using Bss.Identity.Extensions;
using Bss.Identity.Jobs;
using Bss.Identity.Models;
using Bss.Identity.Services;
using Bss.Infrastructure.Commands;
using Bss.Infrastructure.Configuration;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authentication.OAuth;
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

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme =
            option.DefaultChallengeScheme =
            option.DefaultScheme = AuthenticationSchemes.BearerJwt;
        })
        .AddGitHub(SupportedOAuthProviders.Github.ToString(), githubOptions =>
        {
            githubOptions.ClientId = config.Github.ClientId;
            githubOptions.ClientSecret = config.Github.ClientSecret;
            githubOptions.CallbackPath = new PathString(config.Github.CallbackPath);

            githubOptions.Scope.Add("user:email");

            githubOptions.Events = new OAuthEvents
            {
                OnCreatingTicket = OAuthTicketHandlers.OnCreatingOAuthTicket
            };
        })
        .AddGoogle(SupportedOAuthProviders.Google.ToString(), googleOptions =>
        {
            googleOptions.ClientId = config.Google.ClientId;
            googleOptions.ClientSecret = config.Google.ClientSecret;
            googleOptions.CallbackPath = new PathString(config.Google.CallbackPath);

            googleOptions.Scope.Add("email");

            googleOptions.Events = new OAuthEvents
            {
                OnCreatingTicket = OAuthTicketHandlers.OnCreatingOAuthTicket
            };
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
}
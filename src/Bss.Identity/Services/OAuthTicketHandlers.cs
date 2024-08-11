using Bss.Identity.Constants;
using Bss.Identity.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Bss.Identity.Services;

internal static class OAuthTicketHandlers
{
    public async static Task OnCreatingOAuthTicket(OAuthCreatingTicketContext context)
    {
        if (context.Identity == null)
        {
            context.Fail("No identity found");
            return;
        }

        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

        var userName = context.Identity.FindFirst(ClaimTypes.Name)?.Value;
        var email = context.Identity.FindFirst(ClaimTypes.Email)?.Value;

        if (email != null)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    context.Fail("Failed to create user");
                    return;
                }

                await userManager.AddToRoleAsync(user, Roles.User);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            
            context.Success();
        }
        else
        {
            context.Fail("Email claim not found.");
        }
    }
}

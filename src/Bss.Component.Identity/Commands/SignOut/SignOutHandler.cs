using Bss.Component.Identity.Data;
using Bss.Component.Identity.Models;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bss.Identity.Commands.SignOut;

[Authorize]
public class SignOutHandler(
    IUserContext userContext,
    SignInManager<ApplicationUser> signInManager,
    IIdentityDbContext identityDbContext)
{
    public async Task Handle(object _)
    {
        var userId = userContext.UserId;

        await signInManager.SignOutAsync();

        var refreshToken = await identityDbContext.RefreshTokens
            .Where(x => x.UserId == userId)
            .ToListAsync();

        foreach (var token in refreshToken)
        {
            identityDbContext.Delete(token);
        }

        await identityDbContext.SaveChangesAsync();
    }
}
using Bss.Component.Identity.Data;
using Bss.Component.Identity.Models;
using Bss.Component.Identity.Services;
using Microsoft.AspNetCore.Identity;

namespace Bss.Identity.Commands.SignIn;

public class SignInHandler(
    SignInManager<ApplicationUser> signInManager,
    IIdentityDbContext identityDbContext,
    IJwtTokenService jwtTokenService)
{
    public virtual async Task<SignInResponse> Handle(SignInRequest request)
    {
        if (request.Email is null && request.UserName is null)
        {
            throw new UnauthorizedAccessException("Email or UserName must be provided.");
        }

        if (request.Email is not null && request.UserName is not null)
        {
            throw new UnauthorizedAccessException("Email and UserName cannot be provided at the same time.");
        }

        var result = request.Email is not null 
            ? await signInManager.PasswordSignInAsync(request.Email, request.Password, true, false)
            : await signInManager.PasswordSignInAsync(request.UserName!, request.Password, true, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var user = request.Email is not null
            ? await signInManager.UserManager.FindByEmailAsync(request.Email)
            : await signInManager.UserManager.FindByNameAsync(request.UserName!)
                ?? throw new UnauthorizedAccessException("Could not find user.");

        var roles = await signInManager.UserManager.GetRolesAsync(user!);

        var now = DateTime.UtcNow;
        var (tokenId, token, expires) = jwtTokenService.GenerateToken(user!, roles, now);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(), // TODO: Generate a random token
            TokenId = tokenId,
            User = user!,
            CreationDate = now,
            Expires = expires,
        };

        identityDbContext.Push(refreshToken);
        await identityDbContext.SaveChangesAsync();

        return new SignInResponse
        {
            Token = token,
            RefreshToken = refreshToken.Token,
            UserName = user!.UserName!,
            Roles = roles.ToArray(),
            Expires = expires,
            ExpiresIn = (long)(expires - now).TotalSeconds
        };
    }
}
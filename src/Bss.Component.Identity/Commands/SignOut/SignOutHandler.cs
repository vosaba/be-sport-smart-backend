using Bss.Component.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Bss.Identity.Commands.SignOut;

public class SignOutHandler(SignInManager<ApplicationUser> signInManager)
{
    public async Task<Ok> Handle(object _)
    {
        await signInManager.SignOutAsync();
        return TypedResults.Ok();
    }
}
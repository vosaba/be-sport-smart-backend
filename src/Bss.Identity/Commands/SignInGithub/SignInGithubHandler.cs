using Bss.Identity.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace Bss.Identity.Commands.SignInGithub;

[AllowAnonymous]
public class SignInGithubHandler(IOptions<BssIdentityConfiguration> options)
{
    [HttpGet]
    public async Task<ChallengeResult> Handle()
    {
        return new ChallengeResult("Github", new AuthenticationProperties { RedirectUri = options.Value.RedirectUrl });
    }
}

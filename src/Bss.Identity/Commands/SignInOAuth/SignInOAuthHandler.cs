using Bss.Identity.Configuration;
using Bss.Identity.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bss.Identity.Commands.SignInOAuth;

[AllowAnonymous]
public class SignInOAuthHandler(IOptions<BssIdentityConfiguration> options)
{
    // <summary>
    // This handler processes sign-in requests initiated from the UI.
    // It must use a GET request to redirect the user to the OAuth provider's login page, 
    // unlike other commands that typically use POST requests.
    // </summary>
    [HttpGet]
    public ChallengeResult Handle([FromQuery]SupportedOAuthProviders provider)
    {
        return new ChallengeResult(provider.ToString(), new AuthenticationProperties { RedirectUri = options.Value.AfterLoginRedirectUrl });
    }
}
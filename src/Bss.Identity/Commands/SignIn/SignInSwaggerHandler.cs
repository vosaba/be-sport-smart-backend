using Bss.Identity.Data;
using Bss.Identity.Models;
using Bss.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bss.Identity.Commands.SignIn;

// <summary>
// This Handler is responsible for handling the sign in request made from swagger.
// </summary>
public class SignInSwaggerHandler(SignInManager<ApplicationUser> signInManager, IIdentityDbContext identityDbContext, IJwtTokenService jwtTokenService) 
    : SignInHandler(signInManager, identityDbContext, jwtTokenService)
{
    public override Task<SignInResponse> Handle([FromForm] SignInRequest request) => base.Handle(request);
}

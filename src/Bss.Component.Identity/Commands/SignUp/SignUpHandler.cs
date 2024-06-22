using Bss.Component.Identity.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bss.Identity.Commands.SignUp;

public class SignUpHandler(UserManager<ApplicationUser> userManager)
{
    private const string SignUpRole = "User";

    [ProducesResponseType(typeof(OperationErrorResult), 400)]
    public async Task Handle(SignUpRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description), OperationErrorCodes.InvalidRequest);
        }
        
        result = await userManager.AddToRoleAsync(user, SignUpRole);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description));
        }
    }
}
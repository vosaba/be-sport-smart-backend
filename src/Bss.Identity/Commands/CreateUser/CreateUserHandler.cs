using Bss.Identity.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bss.Identity.Commands.CreateUser;

[Authorize(Roles = "Admin")]
public class CreateUserHandler(UserManager<ApplicationUser> userManager)
{
    [ProducesResponseType(typeof(OperationErrorResult), 400)]
    public async Task Handle(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) && string.IsNullOrWhiteSpace(request.Email))
        {
            throw new OperationException("Either UserName or Email must be provided.", OperationErrorCodes.InvalidRequest);
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName ?? request.Email,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description), OperationErrorCodes.InvalidRequest);
        }
        
        result = await userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            throw new OperationException(result.Errors.Select(x => x.Description));
        }
    }
}
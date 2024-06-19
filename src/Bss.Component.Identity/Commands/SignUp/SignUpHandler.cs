using Bss.Component.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bss.Identity.Commands.SignUp;

public class SignUpHandler(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
{
    private const string SignUpRole = "User";

    public async Task<Results<Ok, ValidationProblem>> Handle(SignUpRequest request)
    {
        //var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
        
        //await userStore.SetUserNameAsync(user, request.UserName, CancellationToken.None);
        //await userStore.SetEmailAsync(user, request.Email, CancellationToken.None);

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        //result = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, request.UserName));
        //if (!result.Succeeded)
        //{
        //    return CreateValidationProblem(result);
        //}
        
        result = await userManager.AddToRoleAsync(user, SignUpRole);
        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        return TypedResults.Ok();
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
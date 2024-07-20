using System.ComponentModel.DataAnnotations;

namespace Bss.Identity.Commands.SignIn;

public class SignInRequest
{
    [EmailAddress]
    public string? Email { get; init; } = null;

    public string? UserName { get; init; } = null;

    [Required]
    public string Password { get; init; } = null!;
}

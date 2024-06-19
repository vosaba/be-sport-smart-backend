using System.ComponentModel.DataAnnotations;

namespace Bss.Identity.Commands.SignUp;

public class SignUpRequest
{
    [EmailAddress]
    public string? Email { get; init; }

    [MinLength(3)]
    [MaxLength(50)]
    public string? UserName { get; init; }

    [Required]
    public string Password { get; init; } = null!;
}
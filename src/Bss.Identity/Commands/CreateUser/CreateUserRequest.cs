using System.ComponentModel.DataAnnotations;

namespace Bss.Identity.Commands.CreateUser;

public class CreateUserRequest
{
    [EmailAddress]
    public string? Email { get; init; }

    [MinLength(3)]
    [MaxLength(50)]
    public string? UserName { get; init; }

    public string Role { get; init; } = "User";

    [Required]
    public string Password { get; init; } = null!;
}
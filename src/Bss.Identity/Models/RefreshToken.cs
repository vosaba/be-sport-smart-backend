namespace Bss.Identity.Models;

public class RefreshToken
{
    public Guid Id { get; init; }

    public required string Token { get; init; } = string.Empty;

    public required string? TokenId { get; init; }

    public required DateTime CreationDate { get; init; }

    public required DateTime Expires { get; init; }

    public bool Used { get; private set; }

    public bool Invalidated { get; private set; }

    public Guid UserId { get; init; }

    public required ApplicationUser User { get; init; } = null!;

    public void UseToken()
    {
        Used = true;
    }

    public void InvalidateToken()
    {
        Invalidated = true;
    }
}
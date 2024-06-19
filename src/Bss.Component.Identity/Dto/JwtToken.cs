namespace Bss.Component.Identity.Dto;

public class JwtToken
{
    public string Token { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public string UserName { get; init; } = string.Empty;

    public string[] Roles { get; init; } = [];

    public long ExpiresIn { get; init; }

    public DateTime Expires { get; set; }

}
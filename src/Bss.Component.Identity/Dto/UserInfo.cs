namespace Bss.Component.Identity.Dto;

public class UserInfo
{
    public string? Email { get; init; } = string.Empty;

    public string? UserName { get; init; } = string.Empty;

    public string[] Roles { get; init; } = [];
}
namespace Bss.Component.Identity.Dto;

public class UserInfo
{
    public string UserName { get; init; } = string.Empty;

    public string[] Roles { get; init; } = [];
}
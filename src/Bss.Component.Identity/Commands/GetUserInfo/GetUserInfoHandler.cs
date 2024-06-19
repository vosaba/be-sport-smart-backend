using Bss.Component.Identity.Dto;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Identity.Commands.GetUserInfo;

[Authorize]
public class GetUserInfoHandler(IUserContext userContext)
{
    public Task<UserInfo> Handle(object _)
    {
        return Task.FromResult(new UserInfo
        {
            UserName = userContext.UserName,
            Roles = [..userContext.Roles]
        });
    }
}
using Bss.Identity.Dto;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Identity.Commands.GetUserInfo;

[Authorize]
public class GetUserInfoHandler(IUserContext userContext)
{
    public Task<UserInfo> Handle(object _)
    {
        return Task.FromResult(new UserInfo
        {
            UserName = userContext.UserName,
            Email = userContext.Email,
            Roles = [..userContext.Roles]
        });
    }
}
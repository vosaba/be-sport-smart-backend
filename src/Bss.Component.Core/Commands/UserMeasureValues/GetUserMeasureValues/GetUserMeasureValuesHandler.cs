using Bss.Component.Core.Data;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.UserMeasureValues.GetUserMeasureValues;

[Authorize(Roles = "User")]
public class GetUserMeasureValuesHandler(ICoreDbContext dbContext, IUserContext userContext)
{
    public async Task<Dictionary<string, string>> Handle(object _)
    {
        var userMeasureValues = await dbContext.UserMeasureValues
            .Where(x => x.UserId == userContext.UserId)
            .ToListAsync();

        return userMeasureValues.ToDictionary(x => x.Name, x => x.Value);
    }
}
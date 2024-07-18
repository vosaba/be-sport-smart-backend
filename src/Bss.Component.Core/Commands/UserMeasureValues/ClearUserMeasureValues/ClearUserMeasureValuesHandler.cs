using Bss.Component.Core.Data;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.Component.Core.Commands.UserMeasureValues.ClearUserMeasureValues;

[Authorize(Roles = "User")]
public class ClearUserMeasureValuesHandler(ICoreDbContext dbContext, IUserContext userContext)
{
    public async Task Handle(ClearUserMeasureValuesRequest request)
    {
        var userMeasureValues = await dbContext.UserMeasureValues
            .Where(umv => umv.UserId == userContext.UserId)
            .ToListAsync();

        if (!request.ClearAll)
        {
            userMeasureValues = userMeasureValues
                .Where(umv => request.Names.Contains(umv.Name))
                .ToList();
        }

        foreach (var userMeasureValue in userMeasureValues)
        {
            dbContext.Delete(userMeasureValue);
        }

        await dbContext.SaveChangesAsync();
    }
}

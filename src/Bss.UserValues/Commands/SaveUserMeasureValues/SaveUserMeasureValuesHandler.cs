using Bss.UserValues.Data;
using Bss.UserValues.Models;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bss.UserValues.Commands.UserMeasureValues.SaveUserMeasureValues;

[Authorize(Roles = "User")]
public class SaveUserMeasureValuesHandler(IUserValuesDbContext dbContext, IUserContext userContext)
{
    public async Task Handle(SaveUserMeasureValuesRequest request)
    {
        var userMeasureValues = await dbContext.UserMeasureValues
            .Where(x => x.UserId == userContext.UserId)
            .ToListAsync();

        var userMeasureValuesDictionary = userMeasureValues.ToDictionary(x => x.Name, x => x);

        foreach (var (name, value) in request.Values)
        {
            if (userMeasureValuesDictionary.TryGetValue(name, out var userMeasureValue))
            {
                userMeasureValue.UpdateValue(value);
            }
            else
            {
                userMeasureValue = new UserMeasureValue(userContext.UserId, name, value);
                dbContext.Push(userMeasureValue);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Infrastructure.Errors.Abstractions;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Bss.Component.Core.Commands.UpdateMeasure;

[Authorize(Roles = "Admin")]
public class UpdateMeasureHandler(IUserContext userContext, ICoreDbContext dbContext)
{
    public async Task Handle(UpdateMeasureRequest request)
    {
        var measure = dbContext.Measures
            .SingleOrDefault(x => x.Id == request.Id);

        if (measure is null)
        { 
            throw new NotFoundException<Measure>(request.Id);
        }
        else if (measure.CreatedBy != userContext.UserId)
        {
            throw new OperationException("Measure is owned by another user.", OperationErrorCodes.Forbidden);
        }
        
        measure.Update(request.Name, request.Type, request.InputSource, request.Options, request.Disabled);

        await dbContext.SaveChangesAsync();
    }
}

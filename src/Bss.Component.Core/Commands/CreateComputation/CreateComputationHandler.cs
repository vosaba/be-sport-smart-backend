using Bss.Component.Core.Data;
using Bss.Component.Core.Models;
using Bss.Component.Core.Services;
using Bss.Infrastructure.Identity.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Bss.Component.Core.Commands.CreateComputation;

[Authorize(Roles = "User,Admin")]
public class CreateComputationHandler(
    IUserContext userContext,
    ILogger<CreateComputationHandler> logger, 
    ICoreDbContext dbContext,
    IComputationValidator computationValidator)
{
    public async Task Handle(CreateComputationRequest request)
    {
        var computation = new Computation(
            request.Type,
            request.Name,
            request.Formula,
            userContext.UserName);

        await computationValidator.EnsureValidComputation(computation);

        dbContext.Push(computation);

        await dbContext.SaveChangesAsync();
    }
}

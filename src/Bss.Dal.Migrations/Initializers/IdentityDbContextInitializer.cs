using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bss.Dal.Migrations.Initializers;

public class IdentityDbContextInitializer(ILogger<IdentityDbContext> logger, IdentityDbContext context)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
}
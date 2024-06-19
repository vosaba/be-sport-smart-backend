using Bss.Dal.Configuration;

namespace Bss.Dal.Migrations.ContextFactory;

public class CoreDbContextFactory : DbContextFactoryBase<CoreDbContext>
{
    protected override string GetConnectionString(BssDalConfiguration config)
        => config.ConnectionStrings.BssCore;
}

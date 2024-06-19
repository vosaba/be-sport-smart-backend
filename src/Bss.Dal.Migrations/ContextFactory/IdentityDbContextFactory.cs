using Bss.Dal.Configuration;

namespace Bss.Dal.Migrations.ContextFactory;

public class IdentityDbContextFactory : DbContextFactoryBase<IdentityDbContext>
{
    protected override string GetConnectionString(BssDalConfiguration config)
        => config.ConnectionStrings.BssIdentity;
}
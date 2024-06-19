using Bss.Dal.Configuration;
using Bss.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Bss.Dal.Migrations.ContextFactory;

public abstract class DbContextFactoryBase<T> : IDesignTimeDbContextFactory<T>
    where T : DbContext
{
    public T CreateDbContext(string[] args)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);

        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(path)!)
            .AddJsonFile(Path.GetFileName(path))
            .Build();

        var config = builder.GetConfiguration<BssDalConfiguration>();

        var optionsBuilder = new DbContextOptionsBuilder<T>();
        optionsBuilder.UseNpgsql(
            GetConnectionString(config),
            builder => builder
                .MigrationsAssembly(GetType().Assembly.GetName().Name)
                .SetPostgresVersion(new Version(config.PostgresVersion)));

        return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options)!;
    }

    protected abstract string GetConnectionString(BssDalConfiguration config);
}

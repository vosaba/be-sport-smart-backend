using Bss.Core.Bl.Data;
using Bss.Core.Bl.Models;
using Bss.UserValues.Data;
using Bss.UserValues.Models;
using Microsoft.EntityFrameworkCore;

namespace Bss.Dal;

public class CoreDbContext : DbContext, ICoreDbContext, IUserValuesDbContext
{
    public DbSet<Computation> Computations => Set<Computation>();

    public DbSet<Measure> Measures => Set<Measure>();

    public DbSet<UserMeasureValue> UserMeasureValues => Set<UserMeasureValue>();

    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {
    }

    IQueryable<Computation> ICoreDbContext.Computations => Computations.AsQueryable();

    IQueryable<Measure> ICoreDbContext.Measures => Measures.AsQueryable();

    IQueryable<UserMeasureValue> IUserValuesDbContext.UserMeasureValues => UserMeasureValues.AsQueryable();

    public void Delete<T>(T entity)
        => Remove(entity!);

    public void Push<T>(T entity)
        => Add(entity!);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CoreDbContext).Assembly,
            x => x.Namespace!.EndsWith(nameof(Core)));
        base.OnModelCreating(modelBuilder);
    }
}
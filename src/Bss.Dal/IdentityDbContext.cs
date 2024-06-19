using Bss.Component.Identity.Data;
using Bss.Component.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bss.Dal;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) 
    : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>(options), IIdentityDbContext
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    IQueryable<RefreshToken> IIdentityDbContext.RefreshTokens => RefreshTokens.AsQueryable();

    IQueryable<ApplicationUser> IIdentityDbContext.Users => Users.AsQueryable();

    public void Delete<T>(T entity)
        => Remove(entity!);

    public void Push<T>(T entity)
        => Add(entity!);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentityDbContext).Assembly,
            x => x.Namespace!.EndsWith(nameof(Component.Identity)));
        base.OnModelCreating(modelBuilder);
    }
}
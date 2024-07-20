using Bss.Identity.Models;

namespace Bss.Identity.Data;

public interface IIdentityDbContext
{
    IQueryable<ApplicationUser> Users { get; }

    IQueryable<RefreshToken> RefreshTokens { get; }

    void Push<T>(T entity);

    void Delete<T>(T entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

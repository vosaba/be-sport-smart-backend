using Bss.Component.Core.Models;

namespace Bss.Component.Core.Data;

public interface ICoreDbContext
{
    IQueryable<Computation> Computations { get; }

    IQueryable<Measure> Measures { get; }

    void Push<T>(T entity);

    void Delete<T>(T entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Bss.UserValues.Models;

namespace Bss.UserValues.Data;

public interface IUserValuesDbContext
{
    IQueryable<UserMeasureValue> UserMeasureValues { get; }

    void Push<T>(T entity);

    void Delete<T>(T entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

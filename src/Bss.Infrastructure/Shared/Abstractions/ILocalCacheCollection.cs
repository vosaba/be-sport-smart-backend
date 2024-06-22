namespace Bss.Infrastructure.Shared.Abstractions;

public interface ILocalCacheCollection<TItem>
{
    bool IsEmpty { get; }
    void Add(TItem item);
    void AddRange(IEnumerable<TItem> items);
    void Remove(TItem item);
    TItem? Get(Predicate<TItem> predicate);
    void Clear();
    IEnumerable<TItem> GetAll();
}
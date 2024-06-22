using Bss.Infrastructure.Shared.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Bss.Infrastructure.Shared.Services;

internal class LocalCacheCollection<TItem>(IMemoryCache memoryCache) 
    : ILocalCacheCollection<TItem> 
        where TItem : class
{
    private readonly string _collectionKey = $"{typeof(TItem).Name}_Collection";
    private readonly object _lock = new();

    public bool IsEmpty
    {
        get
        {
            lock (_lock)
            {
                // If the collection is not in the cache, it is considered empty.
                // Used instead of GetCollection() to avoid creating a new collection if it does not exist.
                return !memoryCache.TryGetValue(_collectionKey, out List<TItem>? items) || items!.Count == 0;
            }
        }
    }

    public void Add(TItem item)
    {
        lock (_lock)
        {
            var items = GetCollection();
            items.Add(item);
            memoryCache.Set(_collectionKey, items);
        }
    }

    public void AddRange(IEnumerable<TItem> items)
    {
        lock (_lock)
        {
            var currentItems = GetCollection();
            currentItems.AddRange(items);
            memoryCache.Set(_collectionKey, currentItems);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            memoryCache.Remove(_collectionKey);
        }
    }

    public TItem? Get(Predicate<TItem> predicate)
    {
        lock (_lock)
        {
            var items = GetCollection();
            return items.FirstOrDefault(x => predicate(x));
        }
    }

    public IEnumerable<TItem> GetAll()
    {
        lock (_lock)
        {
            return GetCollection();
        }
    }

    public void Overwrite(IEnumerable<TItem> items)
    {
        lock (_lock)
        {
            memoryCache.Set(_collectionKey, items.ToList());
        }
    }

    public void Remove(TItem item)
    {
        lock (_lock)
        {
            var items = GetCollection();
            items.Remove(item);
            memoryCache.Set(_collectionKey, items);
        }
    }

    private List<TItem> GetCollection()
    {
        if (!memoryCache.TryGetValue(_collectionKey, out List<TItem>? items))
        {
            items = [];
            memoryCache.Set(_collectionKey, items);
        }

        return items!;
    }
}
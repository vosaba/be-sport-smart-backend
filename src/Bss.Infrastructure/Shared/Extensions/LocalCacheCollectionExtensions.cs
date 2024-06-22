using Bss.Infrastructure.Shared.Abstractions;

namespace Bss.Infrastructure.Shared.Extensions;

public static class LocalCacheCollectionExtensions
{
    public static async Task<(List<TItem> items, bool initialized)> InitializeOrGetListAsync<TItem>(
        this ILocalCacheCollection<TItem> cacheCollection,
        Func<Task<IEnumerable<TItem>>> collectionInitializer) where TItem : class
    {
        if (!cacheCollection.IsEmpty)
        {
            return (cacheCollection.GetAll().ToList(), false);
        }

        var items = await collectionInitializer();
        cacheCollection.AddRange(items);

        return (items.ToList(), true);
    }
}
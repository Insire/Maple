namespace Maple.Core
{
    public interface ICachingService<TKey, TObject>
    {
        bool Contains(TKey key);
        bool Add(TKey key, TObject item);

        bool TryGetValue(TKey key, out TObject item);

        void Clear();
        bool Remove(TKey key);
    }
}

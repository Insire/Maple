namespace Maple.Core
{
    public interface ICachingService<TPrimaryKeyType, TViewModel>
    {
        bool Contains(TPrimaryKeyType key);
        bool Add(TPrimaryKeyType key, TViewModel viewModel);

        void Clear();
        bool Remove(TPrimaryKeyType key);
    }
}

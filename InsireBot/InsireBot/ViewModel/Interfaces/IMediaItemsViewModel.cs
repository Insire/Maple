using Maple.Core;
using System.Threading.Tasks;

namespace Maple
{
    public interface IMediaItemsViewModel
    {
        RangeObservableCollection<MediaItem> Items { get; }

        void Add(int playlistId);
        void Load();
        Task LoadAsync();
        void Save();
        Task SaveAsync();
    }
}
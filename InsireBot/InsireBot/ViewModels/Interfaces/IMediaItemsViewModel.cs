using Maple.Core;
using System.Threading.Tasks;

namespace Maple
{
    public interface IMediaItemsViewModel
    {
        RangeObservableCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
        Task LoadAsync();
        Task SaveAsync();
    }
}
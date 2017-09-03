using System.Threading.Tasks;
using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    public interface IMediaItemsViewModel : ISaveableViewModel
    {
        IRangeObservableCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
        Task LoadAsync();
        void Save();
    }
}
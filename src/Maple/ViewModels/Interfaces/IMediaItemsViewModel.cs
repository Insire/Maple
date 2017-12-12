using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Core;

namespace Maple
{
    public interface IMediaItemsViewModel : ISaveableViewModel
    {
        IReadOnlyCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
        Task LoadAsync();
        void Save();
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Maple.Core;

namespace Maple
{
    public interface IMediaItemsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        IReadOnlyCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
    }
}
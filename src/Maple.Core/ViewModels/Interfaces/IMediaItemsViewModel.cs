using System.Collections.Generic;

namespace Maple.Core
{
    public interface IMediaItemsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        IReadOnlyList<MediaItem> Items { get; }

        void Add(Playlist playlist);
    }
}
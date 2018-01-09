using System.Collections.Generic;

namespace Maple.Core
{
    public interface IMediaItemsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        IReadOnlyCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
    }
}
using System.Collections.Generic;

using Maple.Core;

namespace Maple
{
    public interface IMediaItemsViewModel
    {
        IReadOnlyCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
    }
}

using System.Collections.Generic;
using Maple.Core;

namespace Maple
{
    public interface IMediaItemsViewModel : ILoadableViewModel, ISaveableViewModel
    {
        IReadOnlyCollection<MediaItem> Items { get; }

        void Add(Playlist playlist);
    }
}
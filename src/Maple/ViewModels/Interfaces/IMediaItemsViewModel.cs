using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Maple
{
    public interface IMediaItemsViewModel
    {
        ReadOnlyObservableCollection<MediaItem> Items { get; }
    }
}

using Maple.Core;
using System;

namespace Maple
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, ISaveableViewModel, IDisposable
    {
        RangeObservableCollection<MediaPlayer> Items { get; }

        bool Disposed { get; }
    }
}
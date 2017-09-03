using System;
using Maple.Core;
using Maple.Interfaces;

namespace Maple
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, ISaveableViewModel, IDisposable
    {
        IRangeObservableCollection<MediaPlayer> Items { get; }

        bool Disposed { get; }
    }
}
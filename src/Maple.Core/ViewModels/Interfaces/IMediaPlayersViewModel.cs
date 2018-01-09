using System;
using System.Collections.Generic;

namespace Maple.Core
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, ISaveableViewModel, IDisposable
    {
        IReadOnlyCollection<MediaPlayer> Items { get; }
    }
}
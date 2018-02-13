using System;
using System.Collections.Generic;

namespace Maple.Core
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, ISaveableViewModel, IDisposable
    {
        IReadOnlyList<MediaPlayer> Items { get; }
    }
}
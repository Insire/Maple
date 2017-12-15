using System;
using System.Collections.Generic;
using Maple.Core;

namespace Maple
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, ISaveableViewModel, IDisposable
    {
        IReadOnlyCollection<MediaPlayer> Items { get; }
    }
}
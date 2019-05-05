using System;
using System.Collections.Generic;

using Maple.Core;

namespace Maple
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, IDisposable
    {
        IReadOnlyCollection<MediaPlayer> Items { get; }
    }
}

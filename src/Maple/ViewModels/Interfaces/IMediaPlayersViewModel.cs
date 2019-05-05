using System;
using System.Collections.Generic;
using System.ComponentModel;
using Maple.Core;

namespace Maple
{
    public interface IMediaPlayersViewModel : ILoadableViewModel, IDisposable, INotifyPropertyChanged
    {
        IReadOnlyCollection<MediaPlayer> Items { get; }
    }
}

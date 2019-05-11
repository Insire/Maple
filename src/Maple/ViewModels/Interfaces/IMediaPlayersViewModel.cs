using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Maple
{
    public interface IMediaPlayersViewModel : IDisposable, INotifyPropertyChanged
    {
        IReadOnlyCollection<MediaPlayer> Items { get; }
    }
}

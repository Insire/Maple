using System;
using System.ComponentModel;

namespace InsireBot.Core
{
    public interface IMediaItem : IIsSelected, ISequence, INotifyPropertyChanged, IIdentifier
    {
        string Title { get; }
        string Location { get; }

        TimeSpan Duration { get; }

        bool IsRestricted { get; }
    }
}

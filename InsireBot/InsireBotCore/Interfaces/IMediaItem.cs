using System;
using System.ComponentModel;

namespace InsireBotCore
{
    public interface IMediaItem : IIsSelected, IIndex, INotifyPropertyChanged
    {
        string Title { get; }
        string Location { get; }

        TimeSpan Duration { get; }

        bool IsRestricted { get; }

        Guid ID { get; }
    }
}

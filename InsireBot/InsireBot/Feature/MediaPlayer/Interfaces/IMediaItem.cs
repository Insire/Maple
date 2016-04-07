using System;

namespace InsireBot.MediaPlayer
{
    public interface IMediaItem : IIsSelected, IIndex
    {
        string Title { get; }
        string Location { get; }

        TimeSpan Duration { get; }

        bool IsRestricted { get; }

        Guid ID { get; }
    }
}

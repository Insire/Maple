using System;

namespace InsireBot.MediaPlayer
{
    public interface IMediaItem
    {
        string Title { get; }
        string Location { get; }

        TimeSpan Duration { get; }

        bool IsRestricted { get; }

        Guid ID { get; }
    }
}
